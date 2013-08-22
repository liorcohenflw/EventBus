using System;
using System.Text;
using System.Reflection;

namespace NGuava
{
    /// <summary>
    /// This class wrappes the methodInfo and the object which the methodInfo invoked on.
    /// Two EventHandlers are equavelent when they refer the same method on the same instance.
    /// The MethodInfo is a flyweight object similar to String each MethodInfo on a class is uniqueuely formed with the same hashcode.
    /// </summary>
    public class EventHandler
    {
        /// <summary>
        /// The target object is the object  which the method will be invoked on.
        /// </summary>
        private readonly Object target;
        /// <summary>
        /// The object that represent the method in the specific target object.
        /// </summary>
        private readonly MethodInfo method;
        /// <summary>
        /// Constructor that gets target object and method to be invoked on.
        /// </summary>
        /// <param name="target">target that the method is invoked on.</param>
        /// <param name="method">Method of the target object.</param>
        public EventHandler(Object target, MethodInfo method)
        {
            Preconditions.CheckNotNull(target, "EventHandler target can not be null .");
            Preconditions.CheckNotNull(method, "EventHandler method can not be null .");
            this.target = target;
            this.method = method;
        }
        /// <summary>
        /// HandleEvent is invoking the method on the target method with the @event as parameter of the method.
        /// </summary>
        /// <param name="event">Event to handle.It is also acts as a parameter to handler.</param>
        public void HandleEvent(Object @event)
        {
            Preconditions.CheckNotNull(@event, "Event can not be null .");
            try
            {
                method.Invoke(target, new Object[] { @event });
            }
            catch (ArgumentException eArgument)
            {
               
            }
            catch (TargetException eTarget)
            {

            }
            catch (TargetParameterCountException eTargetParameter)
            {

            }
            catch (MethodAccessException eMethodAcces)
            {

            }
            catch (InvalidOperationException eInvalidOperation)
            {

            }
            catch (TargetInvocationException eTargetInvocation)
            {

            }
            //the NotSupportedException is not checked.

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Description of the event handler as string.</returns>
        public override String ToString()
        {
            return "[wrapper " + method.ToString() + "]";
        }
        /// <summary>
        /// The semantics of Equals is special because the target checked with == operator,
        /// it is like this because when we have two different instances (which means to different refernces) of
        /// target (specially if they are the same type)definetly we have two different event handler because the subscriber(target) 
        /// has a state.
        /// The methods are flyweight objects so the next checking is very fast(for specific method in specific object there
        /// is only one EventHandler instance per process, there are no duplications).
        /// </summary>
        /// <param name="obj">Object to be compared.</param>
        /// <returns>If the instances of target and method are the same as obj and obj is EventHandler 
        /// </returns>
        public override Boolean Equals(Object obj)
        {
            if (obj is EventHandler)
            {
                EventHandler that = (EventHandler)obj;
                return target == that.target && method.Equals(that.method);
            }
            return false;
        }
        /// <summary>
        /// An hash code of EventHandler.The hash code is uniqueue depends MethodInfo and Object (target). 
        /// It cause no duplications amoung the application.
        /// </summary>
        /// <returns>Uniququely hash code amoung application.Prevent duplications.</returns>
        public override int GetHashCode()
        {
            return method.GetHashCode() ^ target.GetHashCode();
        }
    }
}
