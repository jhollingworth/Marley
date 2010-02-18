using System;

namespace Marley.Pipeline.Configuration
{
    public class DefaultUriBuilder : IUriBuilder
    {
        private readonly IResourceSpaceConfiguration _configuration;

        public DefaultUriBuilder(IResourceSpaceConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConstructUri<TResource>(TResource resource, object method)
        {
            var metatdata = GetMetdata<TResource>();

            if(IsMethod(method, "get"))
            {
                throw new NotSupportedException("Resource serialization is not supported for GET methods");
            }

            return metatdata.Uri + "/" + metatdata.GetId(resource);
        }

        public string ConstructUri<TResource>(object id, object method)
        {
            var metadata = GetMetdata<TResource>();

            if (false == IsMethod(method, "get"))
            {
                throw new NotSupportedException(string.Format("Method {0} not supported when only Id {1} is supplied", method, id));
            }

            return metadata.Uri + "/" + id;
        }

        private IResourceMetdata GetMetdata<TResource>()
        {
            return _configuration.Get(typeof (TResource));
        }

        private bool IsMethod(object method, string required)
        {
            return string.Compare(method.ToString(), required, true) == 0;
        }
    }
}