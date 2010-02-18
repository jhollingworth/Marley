using System;

namespace Marley.Pipeline.Context
{
    public class ApiContext : IApiContext
    {
        public string ContentType { get; set; }
        public string Accept { get; set; }
        public bool UseFiddler { get; set; }
        public IRequest Request { get; set; }
        public IResponse Response { get; set; }
        public Exception PipelineException { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }
    }
}