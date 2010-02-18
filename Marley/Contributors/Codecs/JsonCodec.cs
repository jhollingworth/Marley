using System.IO;
using System.Text;
using Jayrock.Json;
using Jayrock.Json.Conversion;

namespace Marley.Contributors.Codecs
{
    public class JsonCodec : ICodec
    {
        public string ContentType
        {
            get { return "application/json"; }
        }

        public string Encode(object data)
        {
            var sw = new StringWriter(new StringBuilder());
            var writer = new JsonTextWriter(sw) { PrettyPrint = true };
            JsonConvert.Export(data, writer);
            return sw.ToString();
        }

        public T Decode<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
                return default(T);

            var reader = new JsonTextReader(new StringReader(data));
            return (T)JsonConvert.Import(typeof(T), reader);
        }
    }
}