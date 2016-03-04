namespace EmployeeRecordSystem.Services.Tests
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Text;

    using Contracts;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Xml;
    using Moq;

    [TestClass]
    public class EmployeeSerializationServiceTests
    {
        [TestMethod]
        public void EmployeeSerializationService_WithDefaultConstructor_ShouldReturnValidObject()
        {
            var xmlSerializationServiceMock = new Mock<IXmlSerializationService>();
            var service = new EmployeeSerializationService(xmlSerializationServiceMock.Object);
            Assert.IsNotNull(service, "Service should not be null.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EmployeeSerializationService_WithNullInConstructor_ShouldThorw()
        {
            var service = new EmployeeSerializationService(null);
        }

        [TestMethod]
        public void EmployeeSerializationService_WithNullInConstructor_ShouldThorwWithCorrectParameterName()
        {
            try
            {
                var service = new EmployeeSerializationService(null);
            }
            catch (ArgumentNullException e)
            {
                Assert.AreEqual("service", e.ParamName, "ParamName should be service.");
            }
        }

        [TestMethod]
        public void EmployeeSerializationService_ReadEmployeeDataRecordsFromStringXmlContent_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            string xmlStringContent = File.ReadAllText(fileName, Encoding.UTF8);

            var service = new EmployeeSerializationService(new XmlSerializationService());
            Assert.IsNotNull(service, "Service should not be null");

            var records = service.ReadEmployeeDataRecords(xmlStringContent, Encoding.UTF8).Result;
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
        public void EmployeeSerializationService_ReadEmployeeDataRecordsFromStream_ShouldWork()
        {
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var service = new EmployeeSerializationService(new XmlSerializationService());
                Assert.IsNotNull(service, "Service should not be null");

                var records = service.ReadEmployeeDataRecords(stream).Result;
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
        public void EmployeeSerializationService_SerializeEmployeeDataRecords_ShouldWork()
        {
            var encoding = Encoding.UTF8;
            var fileName = $"{ConfigurationManager.AppSettings["DataFilesPath"]}/employee-records.xml";

            string xmlStringContent = File.ReadAllText(fileName, encoding);

            var service = new EmployeeSerializationService(new XmlSerializationService());
            Assert.IsNotNull(service, "Service should not be null");

            var records = service.ReadEmployeeDataRecords(xmlStringContent, encoding).Result;
            Assert.IsNotNull(records, "Deserialized records should not be null.");

            var newRecords = service.ReadEmployeeDataRecords(
                service.SerializeEmployeeDataRecords(records).Result.OuterXml,
                encoding).Result;

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