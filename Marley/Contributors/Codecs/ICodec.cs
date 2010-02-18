namespace Marley.Contributors.Codecs
{
    public interface ICodec
    {
        string ContentType { get; }
        string Encode(object data);
        T Decode<T>(string data);
    }
}