using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Marley.Contributors;
using Marley.Contributors.Codecs;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;
using Marley.Pipeline.Context;
using Marley.ResourceSpace;
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

            var calender = resourceSpace.Get<WorkspaceCalendar>(132502);
        }

        private class Config : IConfiguration
        {
            public void Configure(IPipelineConfiguration pipeline, IResourceSpaceConfiguration resourceSpace)
            {
                pipeline.RegisterContributor<HuddleUrlBuilderContributor>();
                pipeline.RegisterContributor<RequestBuilderContributor>();
                pipeline.RegisterContributor<RequestSerializerContributor>();
                pipeline.RegisterContributor<RequestExecutorContributor>();
                pipeline.RegisterContributor<ResponseExceptionMapperContributor>();

                pipeline.RegisterCodec<JsonCodec>();

                resourceSpace.Has<WorkspaceCalendar>()
                    .Uri("calendar/workspaces")
                    .ContentType("application/json");
            }
        }

        private class HuddleUrlBuilderContributor : IPipelineContributor
        {
            public void Register(IPipelineConfiguration pipeline)
            {
                pipeline.RegisterBefore<RequestBuilderContributor>(this);
            }

            public PipelineContinuation Execute(IApiContext context)
            {
                context.Request.Uri = "http://api.huddle.dev/v2/" + context.Request.Uri;

                return PipelineContinuation.Continue;
            }
        }

        [XmlRoot]
        public class WorkspaceCalendar
        {
            [XmlAttribute]
            public string Uri { get; set; }

            public SummaryDto Owner { get; set; }

            [XmlArray]
            [XmlArrayItem("Task", typeof(Task))]
            public List<Event> Events { get; set; }
        }

        [XmlType]
        public class SummaryDto
        {
            [XmlAttribute]
            public virtual int Id { get; set; }

            [XmlAttribute]
            public string Uri { get; set; }

            [XmlElement]
            public virtual string DisplayName { get; set; }

        }

        [XmlType]
        public class Task : Event
        {
            public TaskStatus Status { get; set; }
            public DateTime DueDate { get; set; }
            public UserSummaryDto Owner { get; set; }
        }

        [XmlType]
        public abstract class Event
        {
            [XmlAttribute]
            public int Id { get; set; }

            public string Title { get; set; }

            public string Description { get; set; }

            [XmlAttribute]
            public string Uri { get; set; }
        }

        public class UserSummaryDto : SummaryDto
        {
            [XmlAttribute]
            public virtual int Id { get; set; }

            [XmlAttribute]
            public string Uri { get; set; }

            [XmlElement]
            public virtual string DisplayName { get; set; }

            public string LogoPath { get; set; }

            public string EmailAddress { get; set; }
        }

        public enum TaskStatus
        {
            NotStarted,
            Complete
        }
    }
}