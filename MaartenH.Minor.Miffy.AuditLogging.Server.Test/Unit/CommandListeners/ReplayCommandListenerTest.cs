using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MaartenH.Minor.Miffy.AuditLogging.Commands;
using MaartenH.Minor.Miffy.AuditLogging.Events;
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
        private const int WaitTime = 500;

        [TestMethod]
        [DataRow("TestTopic,TopicTest", "TestType", 10392, 29492742)]
        [DataRow("Sometopic", "sometype", 11340392, 2924492742)]
        [DataRow("SRandomTopic", "RandomType", 10, 20)]
        public void HandleCallsFindByOnRepositoryWithExpectedItems(string topics, string types, long fromTimestamp, long toTimeStamp)
        {
            // Arrange
            Mock<IAuditLogItemRepository> repositoryMock = new Mock<IAuditLogItemRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            List<string> topicNames = topics.Split(',').ToList();
            List<string> typeNames = types.Split(',').ToList();

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            ReplayEventsCommand command = new ReplayEventsCommand(toTimeStamp)
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
            Mock<IAuditLogItemRepository> repositoryMock = new Mock<IAuditLogItemRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Returns(Enumerable.Range(0, amount).Select(e => new AuditLogItem()));

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            ReplayEventsCommand command = new ReplayEventsCommand(0);

            // Act
            int result = commandListener.Handle(command).AmountOfEvents;

            // Assert
            Assert.AreEqual(amount, result);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(510)]
        public void FetchedLogItemsArePublished(int amount)
        {
            // Arrange
            Mock<IAuditLogItemRepository> repositoryMock = new Mock<IAuditLogItemRepository>(MockBehavior.Strict);
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Returns(Enumerable.Range(0, amount).Select(e => new AuditLogItem {Id = Guid.NewGuid().ToString(), Data = "test", Topic = "TestTopic", Type = "TestType", TimeStamp = 10}));

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            ReplayEventsCommand command = new ReplayEventsCommand(0);

            // Act
            commandListener.Handle(command);

            Thread.Sleep(WaitTime);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(amount));
        }

        [TestMethod]
        [DataRow("testdata", "testtopic", "testtype", 1028)]
        [DataRow("{random: data}", "some.topic", "RandomType", 213441028)]
        public void FetchedLogItemsArePublishedProperly(string data, string topic, string type, long timestamp)
        {
            // Arrange
            Mock<IAuditLogItemRepository> repositoryMock = new Mock<IAuditLogItemRepository>(MockBehavior.Strict);
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            AuditLogItem auditLogItem = new AuditLogItem
            {
                Id = Guid.NewGuid().ToString(),
                TimeStamp = timestamp,
                Data = data,
                Topic = topic,
                Type = type
            };

            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Returns(new List<AuditLogItem> {auditLogItem});

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            ReplayEventsCommand command = new ReplayEventsCommand(0);

            // Act
            commandListener.Handle(command);

            Thread.Sleep(WaitTime);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(timestamp, $"replay_{topic}", It.IsAny<Guid>(), type, data));
        }

        [TestMethod]
        public void StartEventIsSentOnHandle()
        {
            // Arrange
            Mock<IAuditLogItemRepository> repositoryMock = new Mock<IAuditLogItemRepository>(MockBehavior.Strict);
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Returns(new List<AuditLogItem>());

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            ReplayEventsCommand command = new ReplayEventsCommand(0);

            // Act
            commandListener.Handle(command);

            Thread.Sleep(WaitTime);

            // Assert
            eventPublisherMock.Verify(e => e.Publish(It.IsAny<StartReplayEvent>()));
        }

        [TestMethod]
        public void EndEventIsSentOnHandle()
        {
            // Arrange
            Mock<IAuditLogItemRepository> repositoryMock = new Mock<IAuditLogItemRepository>(MockBehavior.Strict);
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            repositoryMock.Setup(e => e.FindBy(It.IsAny<AuditLogItemCriteria>()))
                .Returns(new List<AuditLogItem>());

            ReplayCommandListener commandListener = new ReplayCommandListener(repositoryMock.Object, eventPublisherMock.Object, new LoggerFactory());

            ReplayEventsCommand command = new ReplayEventsCommand(0);

            // Act
            commandListener.Handle(command);

            Thread.Sleep(WaitTime);

            // Assert
            eventPublisherMock.Verify(e => e.Publish(It.IsAny<EndReplayEvent>()));
        }
    }
}
