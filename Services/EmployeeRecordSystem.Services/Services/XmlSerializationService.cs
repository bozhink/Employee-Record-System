namespace EmployeeRecordSystem.Services
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Xml.XPath;

    using Contracts;

    public class XmlSerializationService : IXmlSerializationService
    {
        public Task<T> Deserialize<T>(Stream xml) where T : class
        {
            return Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(xml);
            });
        }

        public Task<T> Deserialize<T>(XmlReader xml) where T : class
        {
            return Task.Run(() =>
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(xml);
            });
        }

        public async Task<T> Deserialize<T>(string xml, Encoding encoding) where T : class
        {
            using (var stream = new MemoryStream(encoding.GetBytes(xml)))
            {
                return await this.Deserialize<T>(stream);
            }
        }

        public async Task<T> Deserialize<T>(IXPathNavigable xml) where T : class
        {
            return await this.Deserialize<T>(xml.CreateNavigator().ReadSubtree());
        }

        public async Task<string> Serialize(object xml)
        {
            var serializer = new XmlSerializer(xml.GetType());
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, xml);
                stream.Position = 0;
                var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
        }
    }
}