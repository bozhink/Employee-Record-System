namespace EmployeeRecordSystem.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using System.Xml;

    using Contracts;
    using Models.Xml;

    public class EmployeeSerializationService : IEmployeeSerializationService
    {
        private IXmlSerializationService service;

        public EmployeeSerializationService(IXmlSerializationService service)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }

            this.service = service;
        }

        public async Task<DataRecords> ReadEmployeeDataRecords(Stream xml)
        {
            return await this.service.Deserialize<DataRecords>(xml);
        }

        public async Task<DataRecords> ReadEmployeeDataRecords(string xml)
        {
            return await this.service.Deserialize<DataRecords>(xml);
        }

        public async Task<XmlDocument> SerializeEmployeeDataRecords(DataRecords records)
        {
            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true
            };

            string xml = await this.service.Serialize(records);

            document.LoadXml(xml);

            return document;
        }
    }
}