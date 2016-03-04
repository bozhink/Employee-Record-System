namespace EmployeeRecordSystem.Services.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class XmlSerializationServiceTests
    {
        [TestMethod]
        public void XmlSerializationService_WithDefaultConstructor_ShouldReturnValidObject()
        {
            var service = new XmlSerializationService();
            Assert.IsNotNull(service, "Service should not be null");
        }
    }
}