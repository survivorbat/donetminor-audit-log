using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using MaartenH.Minor.Miffy.AuditLogging.Server.Test.Components.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Components.EventListener
{
    [TestClass]
    public class AuditEventLoggingListenerTest
    {
        [TestMethod]
        public void HandlesSavesDeliveredMessage(string message)
        {
            // Arrange
            var repoMock = new Mock<IAuditLogItemRepository>();
            var listener = new AuditEventLoggingListener(repoMock.Object);
            var dummyEvent = new DummyEvent("test.event");

            string jsonEvent = JsonConvert.SerializeObject(dummyEvent);

            // Act
            listener.Handle(jsonEvent);

            // Assert
            repoMock.Setup(e => e.Save(It.IsAny<AuditLogItem>()));
        }
    }
}
