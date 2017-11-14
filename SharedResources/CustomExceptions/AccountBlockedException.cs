using System;

namespace SharedResources.CustomExceptions
{
    public class AccountBlockedException : Exception
    {
        public AccountBlockedException()
            : base("This account has been blocked. Contact the administrator to unblock it")
        {
        }

        public AccountBlockedException(string message)
            : base(message)
        {
        }

        public AccountBlockedException(string message, Exception innException) : base(message, innException)
        {
        }
    }
}