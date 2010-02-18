using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Marley.Contributors.Codecs
{
    public class XmlCodec : ICodec
    {
        #region ICodec Members

        public string ContentType
        {
            get { return "application/xml"; }
        }

        public string Encode(object data)
        {
            var serializer = new XmlSerializer(data.GetType());
            var builder = new StringBuilder();
            var writer = new StringWriterWithEncoding(builder, new UTF8Encoding());
            serializer.Serialize(writer, data);

            return builder.ToString();
        }

        public T Decode<T>(string data)
        {
            var encodedXml = data;
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(encodedXml));
        }

        #endregion
    }
}