using System;

namespace NGuava
{

    /// <summary>
    /// A class for checkings of preconditions.
    /// </summary>
    public sealed class Preconditions
    {
        private Preconditions() { }

        public static void CheckNotNull(Object reference, Object errorMessage)
        {
            if (reference == null)
                throw new NullReferenceException(errorMessage.ToString());
            //change to "null" if refernce of message is null.
        }
        public static void CheckNotNullArgument(Object reference, Object errorMessage)
        {
            if (reference == null)
                throw new ArgumentNullException(errorMessage.ToString());
        }

        public static void CheckArgument(Boolean expression, Object errorMessage)
        {
            if (!expression)
                throw new ArgumentException(errorMessage.ToString());
        }
    }
}