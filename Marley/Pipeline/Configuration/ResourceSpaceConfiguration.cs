using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Marley.Pipeline.Configuration
{
    public class ResourceSpaceConfiguration : IResourceSpaceConfiguration
    {
        public List<IResourceConfiguration> Resources { get; set; }

        public ResourceSpaceConfiguration()
        {
            Resources = new List<IResourceConfiguration>();
        }

        public IResourceConfiguration<T> Has<T>()
        {
            var resource = GetResourceConfig(typeof(T));

            if(resource != null)
                return (IResourceConfiguration<T>)resource;
                
            resource = new ResourceConfiguration<T>();
            Resources.Add(resource);
            return (IResourceConfiguration<T>)resource;
        }

        public IResourceMetdata Get(Type resourceType)
        {
            var resourceConfig = GetResourceConfig(resourceType);

            if(resourceConfig == null)
                throw new ConfigurationException(string.Format("Resource {0} has not been configured", resourceType.Name));

            return new ResourceMetdata
                       {
                           Uri = resourceConfig.Uri,
                           GetId = resourceConfig.GetId,
                           UriBuilder = new DefaultUriBuilder(this),
                           ContentType = resourceConfig.ContentType,
                       };
        }

        private ResourceConfiguration GetResourceConfig(Type type)
        {
            return Resources.Cast<ResourceConfiguration>().FirstOrDefault(r => r.ResourceType == type);
        }
    }
}