namespace EmployeeRecordSystem.Services.Tests
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;
    using System.Xml;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Xml;

    [TestClass]
    public class XmlSerializationServiceTests
    {
        [TestMethod]
        public void XmlSerializationService_WithDefaultConstructor_ShouldReturnValidObject()
        {
            var service = new XmlSerializationService();
            Assert.IsNotNull(service, "Service should not be null");
        }

        [TestMethod]
        public void XmlSerializationService_DeserializeStream_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var service = new XmlSerializationService();
                Assert.IsNotNull(service, "Service should not be null");

                var records = service.Deserialize<DataRecords>(stream).Result;
                Assert.IsNotNull(records, "Deserialized records should not be null.");

                var codes = records.Codes;
                Assert.IsNotNull(codes, "Deserialized codes should not be null.");

                Assert.AreEqual(14, codes.Length, "Number of code items should be 14.");

                var firstCode = codes[0];
                Assert.AreEqual("E0001", firstCode.Id, "Id should match.");
                Assert.AreEqual("Michael Perry", firstCode.EmployeeName, "EmployeeName should match.");
                Assert.AreEqual(new DateTime(1999, 2, 2), firstCode.Details.DateOfJoin, "DateOfJoin should match.");
                Assert.AreEqual(GradeType.A, firstCode.Details.Grade, "Grade should match.");
                Assert.AreEqual(1750.0m, firstCode.Details.Salary, "Salary should match.");
            }
        }

        [TestMethod]
        public void XmlSerializationService_DeserializeXmlStringContent_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            string xmlStringContent = File.ReadAllText(fileName, Encoding.UTF8);

            var service = new XmlSerializationService();
            Assert.IsNotNull(service, "Service should not be null");

            var records = service.Deserialize<DataRecords>(xmlStringContent, Encoding.UTF8).Result;
            Assert.IsNotNull(records, "Deserialized records should not be null.");

            var codes = records.Codes;
            Assert.IsNotNull(codes, "Deserialized codes should not be null.");

            Assert.AreEqual(14, codes.Length, "Number of code items should be 14.");

            var firstCode = codes[0];
            Assert.AreEqual("E0001", firstCode.Id, "Id should match.");
            Assert.AreEqual("Michael Perry", firstCode.EmployeeName, "EmployeeName should match.");
            Assert.AreEqual(new DateTime(1999, 2, 2), firstCode.Details.DateOfJoin, "DateOfJoin should match.");
            Assert.AreEqual(GradeType.A, firstCode.Details.Grade, "Grade should match.");
            Assert.AreEqual(1750.0m, firstCode.Details.Salary, "Salary should match.");
        }

        [TestMethod]
        public void XmlSerializationService_DeserializeIXPathNavigable_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            XmlDocument document = new XmlDocument
            {
                PreserveWhitespace = true
            };

            document.Load(fileName);
            Assert.IsNotNull(document.DocumentElement, "Sample XML document is not loaded.");

            var service = new XmlSerializationService();
            Assert.IsNotNull(service, "Service should not be null");

            var records = service.Deserialize<DataRecords>(document).Result;
            Assert.IsNotNull(records, "Deserialized records should not be null.");

            var codes = records.Codes;
            Assert.IsNotNull(codes, "Deserialized codes should not be null.");

            Assert.AreEqual(14, codes.Length, "Number of code items should be 14.");

            var firstCode = codes[0];
            Assert.AreEqual("E0001", firstCode.Id, "Id should match.");
            Assert.AreEqual("Michael Perry", firstCode.EmployeeName, "EmployeeName should match.");
            Assert.AreEqual(new DateTime(1999, 2, 2), firstCode.Details.DateOfJoin, "DateOfJoin should match.");
            Assert.AreEqual(GradeType.A, firstCode.Details.Grade, "Grade should match.");
            Assert.AreEqual(1750.0m, firstCode.Details.Salary, "Salary should match.");
        }

        [TestMethod]
        public void XmlSerializationService_DeserializeXmlReader_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            XmlReader reader = XmlReader.Create(fileName);
            Assert.IsNotNull(reader, "Sample XML document is not loaded.");

            var service = new XmlSerializationService();
            Assert.IsNotNull(service, "Service should not be null");

            var records = service.Deserialize<DataRecords>(reader).Result;
            Assert.IsNotNull(records, "Deserialized records should not be null.");

            var codes = records.Codes;
            Assert.IsNotNull(codes, "Deserialized codes should not be null.");

            Assert.AreEqual(14, codes.Length, "Number of code items should be 14.");

            var firstCode = codes[0];
            Assert.AreEqual("E0001", firstCode.Id, "Id should match.");
            Assert.AreEqual("Michael Perry", firstCode.EmployeeName, "EmployeeName should match.");
            Assert.AreEqual(new DateTime(1999, 2, 2), firstCode.Details.DateOfJoin, "DateOfJoin should match.");
            Assert.AreEqual(GradeType.A, firstCode.Details.Grade, "Grade should match.");
            Assert.AreEqual(1750.0m, firstCode.Details.Salary, "Salary should match.");
        }

        [TestMethod]
        public void XmlSerializationService_Serialize_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            XmlReader reader = XmlReader.Create(fileName);
            Assert.IsNotNull(reader, "Sample XML document is not loaded.");

            var service = new XmlSerializationService();
            Assert.IsNotNull(service, "Service should not be null");

            var records = service.Deserialize<DataRecords>(reader).Result;
            Assert.IsNotNull(records, "Deserialized records should not be null.");

            string serializedRecords = service.Serialize(records).Result;
            Assert.IsFalse(string.IsNullOrWhiteSpace(serializedRecords), "Serialized records should not be empty string.");

            var newRecords = service.Deserialize<DataRecords>(serializedRecords, Encoding.UTF8).Result;
            Assert.IsNotNull(newRecords, "Deserialized newRecords should not be null.");

            var codes = newRecords.Codes;
            Assert.IsNotNull(codes, "Deserialized codes should not be null.");

            Assert.AreEqual(14, codes.Length, "Number of code items should be 14.");

            var firstCode = codes[0];
            Assert.AreEqual("E0001", firstCode.Id, "Id should match.");
            Assert.AreEqual("Michael Perry", firstCode.EmployeeName, "EmployeeName should match.");
            Assert.AreEqual(new DateTime(1999, 2, 2), firstCode.Details.DateOfJoin, "DateOfJoin should match.");
            Assert.AreEqual(GradeType.A, firstCode.Details.Grade, "Grade should match.");
            Assert.AreEqual(1750.0m, firstCode.Details.Salary, "Salary should match.");

            for (int i = 0; i < records.Codes.Length; ++i)
            {
                Assert.IsTrue(this.CompareDataRecordsCode(records.Codes[i], newRecords.Codes[i]), $"Codes {i} should match.");
            }
        }

        private bool CompareDataRecordsCode(DataRecordsCode code1, DataRecordsCode code2)
        {
            if (code1.Id != code2.Id)
            {
                return false;
            }

            if (code1.EmployeeName != code2.EmployeeName)
            {
                return false;
            }

            if (code1.Details.DateOfJoin != code2.Details.DateOfJoin)
            {
                return false;
            }

            if (code1.Details.Grade != code2.Details.Grade)
            {
                return false;
            }

            if (code1.Details.Salary != code2.Details.Salary)
            {
                return false;
            }

            return true;
        }
    }
}