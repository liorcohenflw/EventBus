using System;
using System.Collections.Generic;

namespace NGuava
{
    /// <summary>
    /// Extension method to remove items from ISet.
    /// </summary>
    public static class RemoveAllExtension
    {
        /// <summary>
        /// Removes from set the items in the enumarble.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="setToRemoveFrom">set to remove from</param>
        /// <param name="itemsToRemove">items to be removed</param>
        public static void RemoveAll<T>(this ISet<T> setToRemoveFrom , IEnumerable<T> itemsToRemove)
        {
            foreach (T item in itemsToRemove)
            {
                setToRemoveFrom.Remove(item);
            }
        }
    }
}
