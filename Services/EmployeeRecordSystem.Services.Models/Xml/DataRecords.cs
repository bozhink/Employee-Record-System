namespace EmployeeRecordSystem.Services.Models.Xml
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "urn:schema-employee-data-records.com")]
    [XmlRoot("data-records", Namespace = "urn:schema-employee-data-records.com", IsNullable = false)]
    public partial class DataRecords
    {
        [XmlElement("code")]
        public DataRecordsCode[] Codes { get; set; }
    }
}