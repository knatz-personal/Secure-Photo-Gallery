using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class DataInsertionException : Exception
    {
        public DataInsertionException()
            : base("Failed to insert a new record.")
        {
        }

        public DataInsertionException(string message)
            : base(message)
        {
        }

        public DataInsertionException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public DataInsertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}