using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class DataDeletionException : Exception
    {
        public DataDeletionException()
            : base("Failed to delete record.")
        {
        }

        public DataDeletionException(string message)
            : base(message)
        {
        }

        public DataDeletionException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public DataDeletionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}