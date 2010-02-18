using System;
using System.Net;
using System.Text;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;

namespace Marley.Contributors
{
    public class RequestExecutorContributor : IPipelineContributor
    {
        #region IPipelineContributor Members

        public void Register(IPipelineConfiguration context)
        {
            context.RegisterAfter<RequestBuilderContributor>(this);
        }

        public PipelineContinuation Execute(IApiContext context)
        {
            WriteDataToStream(context.Request);

            var webRequest = context.Request.Request;

            try
            {
                context.Response.Response = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                context.Response.Response = (HttpWebResponse)ex.Response;
            }

            return PipelineContinuation.Continue;
        }

        private static void WriteDataToStream(IRequest request)
        {
            if (request.Data == null)
                return;

            var content = Encoding.UTF8.GetBytes(request.Data.ToString());

            request.Request.ContentLength = content.Length;
            request.Request.ContentType = request.ContentType;
            request.Request.GetRequestStream().Write(content, 0, content.Length);
            request.Request.GetRequestStream().Flush();
        }

        #endregion
    }
}