using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Host;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Host
{
    [TestClass]
    public class MicroserviceReplayHostBuilderTest
    {
        [TestMethod]
        public void CreateHostReturnsReplayHost()
        {
            // Arrange
            using MicroserviceReplayHostBuilder hostBuilder = new MicroserviceReplayHostBuilder();

            // Act
            IMicroserviceHost host = hostBuilder.CreateHost();

            // Assert
            Assert.IsInstanceOfType(host, typeof(MicroserviceReplayHost));
        }
    }
}
