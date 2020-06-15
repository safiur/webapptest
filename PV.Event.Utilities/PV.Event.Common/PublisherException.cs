using System;

namespace PV.Event.Common
{
    /// <summary>
    /// Publisher Exception Class
    /// </summary>
    public class PublisherException : Exception
    {
        public PublisherException()
        {
        }

        public PublisherException(string message)
            : base(message)
        {
        }

        public PublisherException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
