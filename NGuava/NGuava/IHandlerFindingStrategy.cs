using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGuava
{
    /// <summary>
    /// An interface for finding the methods associated with event type .
    /// It wrrapes each method and target object with EventHandler.
    /// It used for EventBus.
    /// </summary>
    public interface IHandlerFindingStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">object whose handlers are desired</param>
        /// <returns>EventHandler object for each handler method in source . Orgenized by event type.</returns>
        IMultiMap<Type, EventHandler> FindAllHandlers(Object source);
    }
}
