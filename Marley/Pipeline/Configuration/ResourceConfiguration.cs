using System;

namespace Marley.Pipeline.Configuration
{
    public class ResourceConfiguration : IResourceConfiguration
    {
        public ResourceConfiguration(Type type)
        {
            ResourceType = type;
        }
        public Type ResourceType { get; set; }
        public string Uri { get; set; }
        public string ContentType { get; set; }
        public TimeSpan Ttl { get; set; }
        public Func<object, object> GetId { get; set; }
        public IUriBuilder UriBuilder { get; set; }
    }

    public class ResourceConfiguration<TResource> : ResourceConfiguration, IResourceConfiguration<TResource>
    {
        #region IResourceConfiguration Members

        public ResourceConfiguration() : base(typeof(TResource))
        {
        }

        public IResourceConfiguration<TResource> Uri(string uri)
        {
            base.Uri = uri;
            return this;
        }

        public IResourceConfiguration<TResource> ContentType(string contentType)
        {
            base.ContentType = contentType;
            return this;
        }

        public IResourceConfiguration<TResource> Cachable(TimeSpan ttl)
        {
            Ttl = ttl;
            return this;
        }

        public IResourceConfiguration<TResource> UsingUriBuilder(IUriBuilder builder)
        {
            UriBuilder = builder;
            return this;
        }

        public IResourceConfiguration<TResource> Id(Func<TResource, object> getId)
        {
            GetId = new Func<object, object>(o => getId((TResource)o));
            return this;
        }

        #endregion
    }
}