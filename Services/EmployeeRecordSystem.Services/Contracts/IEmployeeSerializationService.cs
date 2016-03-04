namespace EmployeeRecordSystem.Services.Contracts
{
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;

    using Models.Xml;

    public interface IEmployeeSerializationService
    {
        Task<DataRecords> ReadEmployeeDataRecords(string xml, Encoding encoding);

        Task<DataRecords> ReadEmployeeDataRecords(Stream xml);

        Task<XmlDocument> SerializeEmployeeDataRecords(DataRecords records);
    }
}