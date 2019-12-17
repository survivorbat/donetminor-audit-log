using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using MaartenH.Minor.Miffy.AuditLogging.Server.Test.Events;
using Microsoft.Extensions.Logging.Abstractions;
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
            var listener = new AuditEventLoggingListener(repoMock.Object, new NullLoggerFactory());
            var dummyEvent = new DummyEvent("test.event");

            string jsonEvent = JsonConvert.SerializeObject(dummyEvent);

            // Act
            listener.Handle(jsonEvent);

            // Assert
            repoMock.Verify(e => e.Save(It.IsAny<AuditLogItem>()));
        }

        [TestMethod]
        [DataRow("TestId",  "test.topic", 637121867145611215, "SimpleData")]
        [DataRow("123456",  "test.topic", 637121867145611212, "Example Data")]
        public void HandlesSavesDeliveredMessageWithProperData(string id, string topic, long timestamp, string text)
        {
            // Arrange
            var repoMock = new Mock<IAuditLogItemRepository>();
            var listener = new AuditEventLoggingListener(repoMock.Object, new NullLoggerFactory());

            AuditLogItem resultItem = null;
            repoMock.Setup(e => e.Save(It.IsAny<AuditLogItem>()))
                .Callback<AuditLogItem>(item => resultItem = item);

            var inputObject = new
            {
                Id = id,
                TimeStamp = timestamp,
                Data = text,
                Topic = topic
            };

            string input = JsonConvert.SerializeObject(inputObject);

            // Act
            listener.Handle(input);

            // Assert
            Assert.AreEqual(input, resultItem.Data);
        }

        [TestMethod]
        [DataRow("TestId")]
        [DataRow("{\"\"}")]
        public void HandlesSavesThrowsExceptionOnInvalidObject(string data)
        {
            // Arrange
            var repoMock = new Mock<IAuditLogItemRepository>();
            var listener = new AuditEventLoggingListener(repoMock.Object, new NullLoggerFactory());

            // Act
            void Act() => listener.Handle(data);

            // Assert
            Assert.ThrowsException<JsonReaderException>(Act);
        }
    }
}
