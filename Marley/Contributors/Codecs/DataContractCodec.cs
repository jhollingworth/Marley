using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace Marley.Contributors.Codecs
{
    public class DataContractCodec : ICodec
    {
        public string ContentType
        {
            get { return "application/xml"; }
        }

        public string Encode(object data)
        {
            var serializer = new DataContractSerializer(data.GetType());
            var builder = new StringWriter(new StringBuilder());
            var writer = new XmlTextWriter(builder);

            serializer.WriteObject(writer, data);

            return builder.ToString();
        }

        public T Decode<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);

            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(XmlReader.Create(new StringReader(data)));
        }
    }
}
