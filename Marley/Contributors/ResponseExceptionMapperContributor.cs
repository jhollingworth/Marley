using System;
using System.Collections.Generic;
using System.Net;
using Marley.HttpExceptions;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;

namespace Marley.Contributors
{
    public class ResponseExceptionMapperContributor : IPipelineContributor
    {
        private static readonly Dictionary<HttpStatusCode, Type> _statusCodeExceptions = new Dictionary<HttpStatusCode, Type>();

        static ResponseExceptionMapperContributor()
        {
            RegisterException<AuthorizationException>(HttpStatusCode.Unauthorized);
            RegisterException<NetworkConnectionException>(HttpStatusCode.BadGateway);
            RegisterException<BadRequestException>(HttpStatusCode.BadRequest);
            RegisterException<ConflictException>(HttpStatusCode.Conflict);
            RegisterException<AuthorizationException>(HttpStatusCode.Forbidden);
            RegisterException<ObjectDeletedException>(HttpStatusCode.Gone);
            RegisterException<ServerException>(HttpStatusCode.InternalServerError);
            RegisterException<ObjectNotFoundException>(HttpStatusCode.NotFound);
            RegisterException<PaymentRequiredException>(HttpStatusCode.PaymentRequired);
        }

        public void Register(IPipelineConfiguration pipeline)
        {
            pipeline.RegisterAfter<RequestExecutorContributor>(this);
        }

        private static void RegisterException<TException>(HttpStatusCode statusCode)
            where TException : ApiResponseException
        {
            _statusCodeExceptions.Add(statusCode, typeof(TException));
        }

        public PipelineContinuation Execute(IApiContext context)
        {
            if(context.Response.Response == null)
                throw new ApiResponseException(context, "No response returned");

            var statusCode = context.Response.Response.StatusCode;

            if(_statusCodeExceptions.ContainsKey(statusCode))
            {
                var ex = (Exception)Activator.CreateInstance(_statusCodeExceptions[statusCode], context);

                throw ex;
            }

            return PipelineContinuation.Continue;
        }
    }
}