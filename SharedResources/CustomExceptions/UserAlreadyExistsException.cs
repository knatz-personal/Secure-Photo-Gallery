using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() :
            base("A user with this username already exists.Please try again.")
        {
        }

        public UserAlreadyExistsException(string message) : base(message)
        {
        }

        public UserAlreadyExistsException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public UserAlreadyExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}