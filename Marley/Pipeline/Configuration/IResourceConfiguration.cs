using System;

namespace Marley.Pipeline.Configuration
{
    public interface IResourceConfiguration
    {
        Type ResourceType { get; }   
    }

    public interface IResourceConfiguration<TResource> : IResourceConfiguration
    {
        IResourceConfiguration<TResource> Id(Func<TResource, object> getId);
        IResourceConfiguration<TResource> Uri(string uri);
        IResourceConfiguration<TResource> ContentType(string contentType);
        IResourceConfiguration<TResource> Cachable(TimeSpan ttl);
        IResourceConfiguration<TResource> UsingUriBuilder(IUriBuilder builder);
    }
}