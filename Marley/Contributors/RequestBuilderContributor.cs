using System;
using System.Net;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using log4net;

namespace Marley.Contributors
{
    public class RequestBuilderContributor : IPipelineContributor
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (RequestBuilderContributor));

        #region IPipelineContributor Members

        public void Register(IPipelineConfiguration context)
        {
        }

        public PipelineContinuation Execute(IApiContext context)
        {
            ValidateContext(context);

            _log.InfoFormat("Creating a web request for {0}", context.Request.Uri);

            var request = WebRequest.Create(context.Request.Uri);

            request.ContentType = context.ContentType;
            request.Timeout = context.Request.Timeout;
            request.Method = context.Request.HttpMethod;

            if (context.Request.Headers != null)
                foreach (var header in context.Request.Headers)
                    request.Headers[header.Key] = header.Value;

            if (context.UseFiddler)
            {
                _log.InfoFormat("Setting proxy to enable Fiddle");
                request.Proxy = new WebProxy("127.0.0.1", 8888);
            }
            else
            {
                request.Proxy = null;
            }

            context.Request.Request = request;

            return PipelineContinuation.Continue;
        }

        #endregion

        private static void ValidateContext(IApiContext context)
        {
            if (string.IsNullOrEmpty(context.Request.HttpMethod))
            {
                throw new InvalidContextException("The http method was not set in the context", context);
            }

            if (string.IsNullOrEmpty(context.Request.Uri))
            {
                throw new InvalidContextException("The uri was not set in the context", context);
            }

            try
            {
                new Uri(context.Request.Uri);
            }
            catch (UriFormatException ex)
            {
                throw new InvalidContextException("Invalid uri in the context", context, ex);
            }
        }
    }
}