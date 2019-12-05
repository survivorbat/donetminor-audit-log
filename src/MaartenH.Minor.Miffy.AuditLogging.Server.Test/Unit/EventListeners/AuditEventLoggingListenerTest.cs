using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using MaartenH.Minor.Miffy.AuditLogging.Server.Test.Components.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Unit.EventListeners
{
    [TestClass]
    public class AuditEventLoggingListenerTest
    {
        [TestMethod]
        public void HandlesSavesDeliveredMessage()
        {
            // Arrange
            var repoMock = new Mock<IAuditLogItemRepository>();
            var listener = new AuditEventLoggingListener(repoMock.Object);
            var dummyEvent = new DummyEvent("test.event");

            string jsonEvent = JsonConvert.SerializeObject(dummyEvent);

            // Act
            listener.Handle(jsonEvent);

            // Assert
            repoMock.Verify(e => e.Save(It.IsAny<AuditLogItem>()));
        }

        [TestMethod]
        [DataRow("SimpleData")]
        [DataRow("{ 'data': 'Hey!' }")]
        [DataRow("{ 'data': ['Hello', 'World'] }")]
        public void HandlesSavesDeliveredMessageWithProperData(string data)
        {
            // Arrange
            var repoMock = new Mock<IAuditLogItemRepository>();
            var listener = new AuditEventLoggingListener(repoMock.Object);

            AuditLogItem resultItem = null;
            repoMock.Setup(e => e.Save(It.IsAny<AuditLogItem>()))
                .Callback<AuditLogItem>(item => resultItem = item);

            // Act
            listener.Handle(data);

            // Assert
            Assert.IsNotNull(resultItem.DateTime);
            Assert.AreEqual(data, resultItem.Data);
        }
    }
}
