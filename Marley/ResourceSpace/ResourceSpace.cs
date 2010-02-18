using Marley.Contributors.Codecs;
using Marley.Pipeline;
using Marley.Pipeline.Configuration;

namespace Marley.ResourceSpace
{
    public class ResourceSpace : IResourceSpace
    {
        private readonly IPipeline _pipeline;
        private readonly IPipelineConfiguration _pipelineConfiguration;
        private readonly IResourceSpaceConfiguration _resourceSpaceConfiguration;

        public ResourceSpace(IPipeline pipeline, IPipelineConfiguration pipelineConfiguration, IResourceSpaceConfiguration resourceSpaceConfiguration)
        {
            _pipeline = pipeline;
            _pipelineConfiguration = pipelineConfiguration;
            _resourceSpaceConfiguration = resourceSpaceConfiguration;
        }

        private IApiCall ApiCall
        {
            get { return new ApiCall(_pipeline); }
        }

        #region IResourceSpace Members

        public TResource Get<TResource>(object id)
        {
            var response = ApiCall.Get(ConstructUri<TResource>(id));

            return GetCodec<TResource>().Decode<TResource>(response.Data);
        }


        public TResponseResource Delete<TRequestResource, TResponseResource>(TRequestResource resource)
        {
            return Decode<TRequestResource, TResponseResource>(resource, HttpMethod.DELETE);
        }

        public TResponseResource Update<TRequestResource, TResponseResource>(TRequestResource resource)
        {
            return Decode<TRequestResource, TResponseResource>(resource, HttpMethod.PUT);
        }

        public TResponseResource Add<TRequestResource, TResponseResource>(TRequestResource resource)
        {
            return Decode<TRequestResource, TResponseResource>(resource, HttpMethod.POST);
        }

        #endregion

        private TResponseResource Decode<TRequestResource, TResponseResource>(TRequestResource resource, object method)
        {
            var response = ApiCall.Execute(
                ConstructUri(resource, method),
                Encode(resource),
                method
            );

            return GetCodec<TResponseResource>().Decode<TResponseResource>(response.Data);
        }

        private string Encode<T>(T resource)
        {
            return GetCodec<T>().Encode(resource);
        }

        private string ConstructUri<T>(T resource, object method)
        {
            return GetResourceMetadata<T>().UriBuilder.ConstructUri(resource, method);
        }

        private IResourceMetdata GetResourceMetadata<TResource>()
        {
            return _resourceSpaceConfiguration.Get(typeof (TResource));
        }

        private string ConstructUri<TResource>(object id)
        {
            return GetResourceMetadata<TResource>().UriBuilder.ConstructUri<TResource>(id, HttpMethod.GET);
        }

        private ICodec GetCodec<T>()
        {
            var config = _resourceSpaceConfiguration.Get(typeof (T));
            var codec = _pipelineConfiguration.GetCodec(config.ContentType);

            if (codec == null)
                throw new UnknownCodecException(codec.ContentType);

            return codec;
        }
    }
}