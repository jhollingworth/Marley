using System;
using System.Net;
using System.Text;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using log4net;

namespace Marley.Contributors
{
    public class RequestAuthBuilderContributor : IPipelineContributor
    {
        private readonly ILog _log = LogManager.GetLogger(typeof (RequestAuthBuilderContributor));

        #region IPipelineContributor Members

        public void Register(IPipelineConfiguration context)
        {
            context.RegisterAfter<RequestBuilderContributor>(this);
            context.RegisterBefore<RequestExecutorContributor>(this);
        }

        public PipelineContinuation Execute(IApiContext context)
        {
            var request = context.Request.Request;
            request.Headers[HttpRequestHeader.Authorization] = GetAuthorizationCredentials(context.Request);
            return PipelineContinuation.Continue;
        }

        #endregion

        private string GetAuthorizationCredentials(IRequest context)
        {
            var token = context.Username + ":" + context.Password;
            var tokenBytes = Encoding.ASCII.GetBytes(token);
            var credentials = "Basic " + Convert.ToBase64String(tokenBytes);

            _log.InfoFormat("Authorization: {0} ({1})", token, credentials);

            return credentials;
        }
    }
}