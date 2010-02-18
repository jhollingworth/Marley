using System;
using System.Linq;
using Marley.Contributors.Codecs;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using Moq;
using NUnit.Framework;

namespace Marley.Tests.Pipeline.Configuration
{
    [TestFixture]
    public class PipelineConfigurationTests
    {
        private PipelineConfiguration _pipeline;

        [SetUp]
        public void SetUp()
        {
            _pipeline = new PipelineConfiguration(new Mock<IResourceSpaceConfiguration>().Object);
        }

        [Test]
        public void can_register_a_codec()
        {
            _pipeline.RegisterCodec<TestCodec>();

            Assert.IsTrue(pipeline_contains_the_codec<TestCodec>());
        }

        [Test]
        public void can_get_a_codec_by_content_type()
        {
            _pipeline.RegisterCodec<TestCodec>();

            Assert.IsInstanceOf<TestCodec>(_pipeline.GetCodec(new TestCodec().ContentType));
        }

        [Test]
        public void can_remove_codec()
        {
            _pipeline.RegisterCodec<TestCodec>();
            Assert.IsTrue(pipeline_contains_the_codec<TestCodec>());
            _pipeline.RemoveCodec<TestCodec>();
            Assert.IsFalse(pipeline_contains_the_codec<TestCodec>());
        }

        [Test]
        public void can_register_contributor()
        {
            given_this_contributor_is_registered<TestContributor>();
            Assert.IsTrue(the_pipeline_contains_the_contributor<TestContributor>());
        }

        [Test]
        public void if_a_contributor_is_registered_more_than_once_only_one_instance_is_added_to_the_pipeline()
        {
            given_this_contributor_is_registered<TestContributor>();
            given_this_contributor_is_registered<TestContributor>();
            Assert.AreEqual(1, _pipeline.Contributors.Count());
        }

        [Test]
        public void if_a_codec_is_registered_more_than_once_only_one_instance_is_added_to_the_pipeline()
        {
            _pipeline.RegisterCodec<TestCodec>();
            _pipeline.RegisterCodec<TestCodec>();
            Assert.AreEqual(1, _pipeline.Codecs.Count());
        }

        [Test]
        public void can_remove_contributor()
        {
            given_this_contributor_is_registered<TestContributor>();
            Assert.IsTrue(the_pipeline_contains_the_contributor<TestContributor>());
            _pipeline.RemoveContributor<TestContributor>();
            Assert.IsFalse(the_pipeline_contains_the_contributor<TestContributor>());
        }

        [Test]
        public void can_add_contributor_after_another()
        {
            given_this_contributor_is_registered<TestContributor>(RegistrationMetadata.RegistrationType.After);
            then_pipeline_has_registration_metadata_for<TestContributor>(RegistrationMetadata.RegistrationType.After);
        }

        [Test]
        public void can_add_contributor_before_antother()
        {
            given_this_contributor_is_registered<TestContributor>(RegistrationMetadata.RegistrationType.Before);
            then_pipeline_has_registration_metadata_for<TestContributor>(RegistrationMetadata.RegistrationType.Before);
        }

        [Test]
        public void can_get_pipeline_from_configuration()
        {
            given_this_contributor_is_registered<TestContributor>();
            given_this_codec_is_registered<TestCodec>();

            var pipeline = _pipeline.GetPipeline();

            Assert.IsTrue(pipeline.Codecs.Any(c => c is TestCodec));
            Assert.IsTrue(pipeline.Contributors.Any(c => c is TestContributor));
        }

        private void given_this_codec_is_registered<TCodec>() where TCodec : ICodec, new()
        {
            _pipeline.RegisterCodec<TCodec>();
        }

        private void then_pipeline_has_registration_metadata_for<T>(RegistrationMetadata.RegistrationType type) where T : IPipelineContributor, new()
        {
            var metadata = _pipeline.Metadata.SingleOrDefault(r => r.Contributor.GetType() == typeof (T));

            Assert.IsNotNull(metadata);
            Assert.AreEqual(metadata.Type, type);
        }

        private void given_this_contributor_is_registered<T>(RegistrationMetadata.RegistrationType registrationType) where T : IPipelineContributor, new()
        {
            switch (registrationType)
            {
                case RegistrationMetadata.RegistrationType.Before:
                    _pipeline.RegisterBefore<T>(new T());
                    break;
                case RegistrationMetadata.RegistrationType.After:
                    _pipeline.RegisterAfter<T>(new T());
                    break;
            }
        }

        private void given_this_contributor_is_registered<TContributor>()
            where TContributor : IPipelineContributor, new()
        {
            _pipeline.RegisterContributor<TContributor>();
        }

        private bool the_pipeline_contains_the_contributor<TContributor>()
            where TContributor : IPipelineContributor
        {
            return _pipeline.Contributors.Any(c => c is TContributor);
        }

        private class TestContributor : IPipelineContributor
        {
            public void Register(IPipelineConfiguration context)
            {
            }

            public PipelineContinuation Execute(IApiContext context)
            {
                return PipelineContinuation.Continue;
            }
        }

        private class OtherTestContributor : IPipelineContributor
        {
            public void Register(IPipelineConfiguration pipeline)
            {
            }

            public PipelineContinuation Execute(IApiContext context)
            {
                return PipelineContinuation.Continue;
            }
        }

        private class TestCodec : ICodec
        {

            public string ContentType
            {
                get { return "application/foo"; }
            }

            public string Encode(object data)
            {
                return string.Empty;
            }

            public T Decode<T>(string data)
            {
                return default(T);
            }
        }

        private bool pipeline_contains_the_codec<TCodec>()
            where TCodec : ICodec
        {
            return _pipeline.Codecs.Any(c => c is TestCodec);
        }
    }
}