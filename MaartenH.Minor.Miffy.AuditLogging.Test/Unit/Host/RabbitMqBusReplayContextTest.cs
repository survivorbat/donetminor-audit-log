using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Host
{
    [TestClass]
    public class RabbitMqBusReplayContextTest
    {
        [TestMethod]
        [DataRow("TestExchange-")]
        [DataRow("SecretStuff-")]
        public void ReplyExchangePrefixIsProperlySet(string exchangePrefix)
        {
            // Arrange
            Mock<IConnection> connectionMock = new Mock<IConnection>();

            // Act
            using RabbitMqBusReplayContext context = new RabbitMqBusReplayContext(connectionMock.Object, "test-exchange", exchangePrefix);

            // Assert
            Assert.AreEqual(exchangePrefix, context.ReplayExchangePrefix);
        }
    }
}
