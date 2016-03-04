namespace EmployeeRecordSystem.Services.Contracts
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.XPath;

    public interface IXmlSerializationService
    {
        Task<T> Deserialize<T>(IXPathNavigable xml) where T : class;

        Task<T> Deserialize<T>(string xml, Encoding encoding) where T : class;

        Task<T> Deserialize<T>(Stream xml) where T : class;

        Task<T> Deserialize<T>(XmlReader xml) where T : class;

        Task<string> Serialize(object xml);
    }
}