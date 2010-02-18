using System;

namespace Marley.Pipeline.Context
{
    public class ApiContext : IApiContext
    {
        public ApiContext()
        {
        }

        public ApiContext(IRequest request, IResponse response)
        {
            Request = request;
            Response = response;
        }

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