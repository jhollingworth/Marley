using System;
using System.Linq;
using Marley.Pipeline.Configuration;
using NUnit.Framework;

namespace Marley.Tests.Pipeline.Configuration
{
    [TestFixture]
    public class ResourceSpaceConfigurationTests
    {
        private IResourceSpaceConfiguration _resourceSpace;

        [SetUp]
        public void SetUp()
        {
            _resourceSpace = new ResourceSpaceConfiguration();
        }

        [Test]
        public void when_resource_space_registers_resource()
        {
            _resourceSpace.Has<User>();

            resource_space_should_have_one_resource_that(r => r.ResourceType == typeof(User));
        }

        [Test]
        public void when_resource_space_registers_resource_can_specify_uri()
        {
            _resourceSpace.Has<User>()
                .Uri("/User");

            resource_space_should_have_one_resource_that(r => r.Uri == "/User");
        }

        [Test]
        public void when_registerting_resource_can_specify_content_type()
        {
            _resourceSpace.Has<User>()
                .ContentType("application/json");

            resource_space_should_have_one_resource_that(r => r.ContentType == "application/json");
        }

        [Test]
        public void when_registering_resource_can_specify_if_it_is_cacheable()
        {
            _resourceSpace.Has<User>()
                .Cachable(new TimeSpan(1, 0, 0));

            resource_space_should_have_one_resource_that(r => r.Ttl.Hours == 1);
        }

        [Test]
        public void when_resource_is_registered_can_retrive_it()
        {
            _resourceSpace.Has<User>();

            var config = _resourceSpace.Get(typeof (User));

            Assert.IsNotNull(config);
        }

        [Test]
        public void when_registering_resources_multiple_times_same_resource_config_should_be_returned()
        {
            var config = _resourceSpace.Has<User>();

            Assert.AreEqual(config, _resourceSpace.Has<User>());
        }

        private void resource_space_should_have_one_resource_that(Func<ResourceConfiguration, bool> should)
        {
            Assert.AreEqual(_resourceSpace.Resources.Cast<ResourceConfiguration>().Count(should), 1);
        }

        private class User
        {
        }
    }
}