using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() :
            base("A user with this username already exists.Please try again.")
        {
        }

        public ItemNotFoundException(string message)
            : base(message)
        {
        }

        public ItemNotFoundException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public ItemNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}