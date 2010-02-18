using System;
using System.Collections.Generic;

namespace Marley.Pipeline.Configuration
{
    public interface IResourceSpaceConfiguration
    {
        List<IResourceConfiguration> Resources { get; }
        IResourceConfiguration<T> Has<T>();
        IResourceMetdata Get(Type type);
    }

    public interface IResourceMetdata 
    {
        string ContentType { get; }
        string Uri { get; }
        IUriBuilder UriBuilder { get; set; }
        Func<object, object> GetId { get; set; }
    }

    public class ResourceMetdata : IResourceMetdata
    {
        public string ContentType { get; set; }
        public string Uri { get; set; }
        public IUriBuilder UriBuilder { get; set; }
        public Func<object, object> GetId { get; set; }
    }
}