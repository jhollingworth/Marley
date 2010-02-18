using System;
using Marley.Contributors;
using Marley.Contributors.Codecs;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using Marley.ResourceSpace;
using Moq;
using NUnit.Framework;

namespace Marley.Tests
{
    [TestFixture]
    public class BasicTests
    {
        [Test]
        public void it_can_do_stuff()
        {
            var factory = new ResourceSpaceFactory(new Config());
            var resourceSpace = factory.GetResourceSpace();

//            var user = resourceSpace.Get<User>(1);
        }

        private class Config : IConfiguration
        {
            public void Configure(IPipelineConfiguration pipeline, IResourceSpaceConfiguration resourceSpace)
            {
                pipeline.RegisterContributor<RequestBuilderContributor>();
                pipeline.RegisterContributor<RequestExecutorContributor>();
                pipeline.RegisterCodec<JsonCodec>();

                resourceSpace.Has<User>()
                    .Uri("http://foo.com/User")
                    .Id(u => u.Id)
                    .ContentType("application/json");
            }
        }

        private class MockRequestExecutorContributor : IPipelineContributor
        {
            public void Register(IPipelineConfiguration pipeline)
            {
                pipeline.RegisterAfter<RequestBuilderContributor>(this);
            }

            public PipelineContinuation Execute(IApiContext context)
            {
                throw new NotImplementedException();
            }
        }

        public class User
        {
            public int Id { get; set; }
        }
    }
}