using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class DataUpdateException : Exception
    {
        public DataUpdateException() : base("An error occurred while attempting to update records.")
        {
        }

        public DataUpdateException(string message) : base(message)
        {
        }

        public DataUpdateException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public DataUpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}