namespace Marley.ResourceSpace
{
    public interface IResourceSpace
    {
        TResource Get<TResource>(object id);
        TResponseResource Delete<TRequestResource, TResponseResource>(TRequestResource resource);
        TResponseResource Update<TRequestResource, TResponseResource>(TRequestResource resource);
        TResponseResource Add<TRequestResource, TResponseResource>(TRequestResource resource);
    }
}