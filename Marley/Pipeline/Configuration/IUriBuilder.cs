namespace Marley.Pipeline.Configuration
{
    public interface IUriBuilder
    {
        string ConstructUri<TResource>(TResource resource, object method);
        string ConstructUri<TResource>(object id, object method);
    }
}