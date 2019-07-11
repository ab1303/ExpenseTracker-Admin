using System;

namespace PartnerUser.Repositories.Exceptions
{
    public class DuplicateKeyException : Exception
    {
        public DuplicateKeyException(string message) : base(message)
        {
        }

        public DuplicateKeyException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DuplicateKeyException()
        {
        }
    }
}
