using System.Collections.Generic;
using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Commands;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.CommandListeners;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Moq;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Unit.CommandListeners
{
    [TestClass]
    public class ReplayCommandListenerTest
    {
        [TestMethod]
        [DataRow("TestTopic,TopicTest", "TestType", 10392, 29492742)]
        [DataRow("Sometopic", "sometype", 11340392, 2924492742)]
        [DataRow("SRandomTopic", "RandomType", 10, 20)]
        public void HandleCallsFindByOnRepositoryWithExpectedItems(string topics, string types, long fromTimestamp, long toTimeStamp)
        {
            // Arrange
            var repositoryMock = new Mock<IAuditLogItemRepository>();
            var eventPublisherMock = new Mock<IEventPublisher>();

            List<string> topicNames = topics.Split(',').ToList();
            List<string> typeNames = types.Split(',').ToList();

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            var command = new ReplayEventsCommand(toTimeStamp)
            {
                Topics = topicNames,
                Types = typeNames,
                FromTimeStamp = fromTimestamp
            };

            AuditLogItemCriteria criteria = null;
            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Callback<AuditLogItemCriteria>(e => criteria = e);

            // Act
            commandListener.Handle(command);

            // Assert
            Assert.AreEqual(fromTimestamp, criteria.FromTimeStamp);
            Assert.AreEqual(toTimeStamp, criteria.ToTimeStamp);
            Assert.AreEqual(topicNames, criteria.Topics);
            Assert.AreEqual(typeNames, criteria.Types);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(510)]
        public void AmountOfEventsAreProperlyReturned(int amount)
        {
            // Arrange
            var repositoryMock = new Mock<IAuditLogItemRepository>();
            var eventPublisherMock = new Mock<IEventPublisher>();

            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Returns(Enumerable.Range(0, amount).Select(e => new AuditLogItem()));

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            var command = new ReplayEventsCommand(0);

            // Act
            int result = commandListener.Handle(command).AmountOfEvents;

            // Assert
            Assert.AreEqual(amount, result);
        }
    }
}
