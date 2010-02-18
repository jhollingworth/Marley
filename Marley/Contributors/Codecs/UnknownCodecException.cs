using System;
using System.Runtime.Serialization;

namespace Marley.Contributors.Codecs
{
    [Serializable]
    public class UnknownCodecException : Exception
    {
        public UnknownCodecException(string contentType) 
            : base(string.Format("Could not find a codec for the content type " + contentType))
        {
        }
    }
}