namespace EmployeeRecordSystem.Services.Models.Xml
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "urn:schema-employee-data-records.com")]
    [XmlRoot("code", Namespace = "urn:schema-employee-data-records.com", IsNullable = false)]
    public partial class DataRecordsCode
    {
        [XmlElement("details")]
        public DataRecordsCodeDetails Details { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("employee-name", DataType = "string")]
        public string EmployeeName { get; set; }
    }
}