using System;
using System.Runtime.Serialization;

namespace Marley.Pipeline.Configuration
{
    [Serializable]
    public class ConflictingMetadataException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ConflictingMetadataException()
        {
        }

        public ConflictingMetadataException(string message)
            : base(message)
        {
        }

        public ConflictingMetadataException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ConflictingMetadataException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}