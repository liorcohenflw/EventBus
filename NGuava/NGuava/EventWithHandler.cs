using System;

namespace NGuava
{   
    /// <summary>
    /// This class is used when the event as parameter and the handler is associated in a queue context.
    /// </summary>
    public class EventWithHandler
    {
        /// <summary>
        /// Event which is also the parameter that the handler is invoked on it.
        /// </summary>
        private readonly Object @event;
        /// <summary>
        /// Handler that gets the event as parameter and can invoke method of specific object on the event as parameter.
        /// </summary>
        private readonly EventHandler handler;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="event"></param>
        /// <param name="handler"></param>
        public EventWithHandler(Object @event, EventHandler handler)
        {
            Preconditions.CheckArgument(@event != null, "Event object is null.");
            Preconditions.CheckArgument(@event != null, "EventHandler is null.");
            this.@event = @event;
            this.handler = handler;
            
        }
        /// <summary>
        /// 
        /// </summary>
        public Object Event { get { return @event; } }
        /// <summary>
        /// 
        /// </summary>
        public EventHandler Handler { get { return handler;} }
    }
}
