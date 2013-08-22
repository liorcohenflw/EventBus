using System;
using System.Collections.Generic;
using System.Threading;

namespace NGuava
{
    /// <summary>
    /// A tool to subscribe to events and publish events.
    /// </summary>
    public  class EventBus : IEventBus 
    {
        /// <summary>
        ///Finder of methods with specific attribute. 
        /// </summary>
        private readonly IHandlerFindingStrategy finder;
        /// <summary>
        /// MultiMap with key of event type and values os event handlers. 
        /// </summary>
        private readonly IMultiMap<Type, EventHandler> handlersByType;
        /// <summary>
        /// Locker of reading and writing from the MultiMap with readers writer policy.
        /// </summary>
        private readonly ReaderWriterLockSlim handlersByTypeLock;
        /// <summary>
        /// TODO : check the memory issues of threadLocal (Remove).
        /// 
        /// </summary>
        private readonly ThreadLocal<Queue<EventWithHandler>> eventsToDispatch;
        /// <summary>
        /// Thread local boolean that prevents reenterency of thread.
        /// </summary>readonly
        private readonly ThreadLocal<Boolean> isDispatching;
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EventBus() : this(new AttributeHandlerFinder())
        {
        }
        /// <summary>
        /// Constructor that gets the finder of methods marked with specific atrributes.
        /// </summary>
        /// <param name="finder"></param>
        public EventBus(IHandlerFindingStrategy finder)
        {
            //TODO : readerwriterlockslim , conccurentdictionary.
            //Replace with ninject tools.
            this.finder = finder;
            handlersByType = HashMultiMap<Type, EventHandler>.Create();
            handlersByTypeLock = new ReaderWriterLockSlim();
            eventsToDispatch = new ThreadLocal<Queue<EventWithHandler>>(() => { return new Queue<EventWithHandler>(); });
            isDispatching = new ThreadLocal<Boolean>(() => { return false; });                                        
        }
        /// <summary>
        /// Register the instance as subscriber through atrribute subscribe.
        /// </summary>
        /// <param name="object">Instance to registred as subscriber to events.</param>
        public void Register(Object @object)
        {
            IMultiMap<Type, EventHandler> methodsInSubscriber = finder.FindAllHandlers(@object);
            handlersByTypeLock.EnterWriteLock();
            try
            {
                handlersByType.AddAll(methodsInSubscriber);
            }
            catch (Exception e)
            {
                //add  logger message.
            }
            finally
            {
                handlersByTypeLock.ExitWriteLock();
            }
        }
        /// <summary>
        /// Unregister the instance as subscriber.
        /// </summary>
        /// <Preconditions>
        /// @object is not null.
        /// The method Regiter is activated on the instance @object.
        /// </Preconditions>
        /// <param name="object">Instance to be unregistered</param>
        public void UnRegister(Object @object)
        {
            IMultiMap<Type, EventHandler> methodsInListener = finder.FindAllHandlers(@object);
            ISet<EventHandler> eventMethodsInListner = null;
            foreach (Type eventType in methodsInListener.Keys)
            {
               eventMethodsInListner  = methodsInListener.GetSet(eventType);
               handlersByTypeLock.EnterWriteLock();
               try
               {
                   ISet<EventHandler> currentHandlers = handlersByType.GetSet(eventType);
                   if (!eventMethodsInListner.IsSubsetOf(currentHandlers))
                       throw new ArgumentException("missing event handlers for an annotated method. Is " + @object.ToString() + " registered?");
                   currentHandlers.RemoveAll<EventHandler>(eventMethodsInListner);
               }
               finally
               {
                   handlersByTypeLock.ExitWriteLock();
               }
            }
 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        public void Post(Object @event)
        {
            handlersByTypeLock.EnterReadLock();
            try
            {
                var handlers = handlersByType.Get(@event.GetType());
                foreach (EventHandler eventHandler in handlers)
                {
                    EnqueueEvent(@event, eventHandler);
                }
            }
            finally
            {
                handlersByTypeLock.ExitReadLock();
            }
            DispatchQueuedEvents();
        }
        /// <summary>
        /// Entering event and handler to the queue of current  thread.
        /// </summary>
        /// <param name="event">Event to enter to queue.</param>
        /// <param name="handler">Handler to enter to queue.</param>
        private void EnqueueEvent(Object @event, EventHandler handler)
        {
            eventsToDispatch.Value.Enqueue(new EventWithHandler(@event , handler));
        }
        /// <summary>
        /// Dispatch all events from thread local queue.
        /// If thread is dipatching when entring to this method , reenterency prevented and the events queued will be dispatched 
        /// in the current run of thread on this method.
        /// </summary>
        private void DispatchQueuedEvents()
        {
            if (isDispatching.Value)
                return;
            isDispatching.Value = true;
            try
            {
                Queue<EventWithHandler> events = eventsToDispatch.Value;
                EventWithHandler currentEventWithHandler = default(EventWithHandler);
                while (events.Count > 0)
                {
                    currentEventWithHandler = events.Dequeue();
                    DispatchWithOutExceptionThrown(currentEventWithHandler.Event, currentEventWithHandler.Handler);
                }
            }
            finally
            {
                eventsToDispatch.Value = new Queue<EventWithHandler>();
                isDispatching.Value = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="eventHandler"></param>
        private void DispatchWithOutExceptionThrown(Object @event , EventHandler eventHandler)
        {
            try
            {
                eventHandler.HandleEvent(@event);
            }
            catch (Exception e)
            {
            }
        }
    }
}
