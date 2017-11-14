using System;

namespace SharedResources.CustomExceptions
{
    public class DuplicateDataException : Exception
    {
        public DuplicateDataException()
             : base("Duplicate records detected!")
        {
        }

        public DuplicateDataException(string message)
            : base(message)
        {
        }

        public DuplicateDataException(string message, Exception innException)
            : base(message, innException)
        {
        }
    }
}