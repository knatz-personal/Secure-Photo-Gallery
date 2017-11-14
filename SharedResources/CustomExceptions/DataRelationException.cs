using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class DataRelationException : Exception
    {
        public DataRelationException() : base("Modification of this attribute is restricted because it is linked to other resources")
        {
        }

        public DataRelationException(string message) : base(message)
        {
        }

        public DataRelationException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public DataRelationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}