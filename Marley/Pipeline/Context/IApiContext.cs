using System;

namespace Marley.Pipeline.Context
{
    public interface IApiContext
    {
        string Accept { get; set; }
        bool UseFiddler { get; set; }
        IRequest Request { get; set; }
        IResponse Response { get; set; }

        /// <summary>
        /// If an exception occurs in the pipeline 
        /// </summary>
        Exception PipelineException { get; set; }
    }
}