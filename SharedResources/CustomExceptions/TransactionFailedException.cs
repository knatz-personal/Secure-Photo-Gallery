using System;
using System.Runtime.Serialization;

namespace SharedResources.CustomExceptions
{
    public class TransactionFailedException : Exception
    {
        public TransactionFailedException() :
            base("A transaction has failed. All actions have been reversed")
        {
        }

        public TransactionFailedException(string message) : base(message)
        {
        }

        public TransactionFailedException(string message, Exception inner) :
            base(message, inner)
        {
        }

        public TransactionFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}