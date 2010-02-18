using System;
using System.Runtime.Serialization;

namespace Marley.Pipeline.Context
{
    [Serializable]
    public class InvalidContextException : Exception
    {
        private readonly IApiContext _context;

        public InvalidContextException(string message, IApiContext context) : base(message)
        {
            _context = context;
        }

        public InvalidContextException(string message, IApiContext context, Exception innerException)
            : base(message, innerException)
        {
            _context = context;
        }

        protected InvalidContextException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }

        public IApiContext Context
        {
            get { return _context; }
        }
    }
}