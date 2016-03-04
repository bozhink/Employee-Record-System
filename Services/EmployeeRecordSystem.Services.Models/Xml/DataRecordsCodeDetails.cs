namespace EmployeeRecordSystem.Services.Models.Xml
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlType(AnonymousType = true, Namespace = "urn:schema-employee-data-records.com")]
    [XmlRoot("details", Namespace = "urn:schema-employee-data-records.com", IsNullable = false)]
    public partial class DataRecordsCodeDetails
    {
        [XmlAttribute("date-of-join", DataType = "date")]
        public DateTime DateOfJoin { get; set; }

        [XmlAttribute("grade")]
        public GradeType Grade { get; set; }

        [XmlAttribute("salary", DataType = "decimal")]
        public decimal Salary { get; set; }
    }
}