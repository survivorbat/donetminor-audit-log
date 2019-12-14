using MaartenH.Minor.Miffy.AuditLogging.Commands;
using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Commands;
using Moq;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Commands
{
    [TestClass]
    public class ReplayCommandPublisherTest
    {
        [TestMethod]
        public void InitiateCallsPauseOnHostIfNotPausedAndListening()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(true);
            hostMock.SetupGet(e => e.IsPaused).Returns(false);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.Pause());
        }

        [TestMethod]
        public void InitiateDoesNotCallPauseOnHostIfPausedAndListening()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(true);
            hostMock.SetupGet(e => e.IsPaused).Returns(true);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.Pause(), Times.Never);
        }

        [TestMethod]
        public void InitiateDoesNotCallPauseOnHostIfNotPausedAndNotListening()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(false);
            hostMock.SetupGet(e => e.IsPaused).Returns(false);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.Pause(), Times.Never);
        }

        [TestMethod]
        public void InitiateDoesNotCallPauseOnHostIfPausedAndNotListening()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(false);
            hostMock.SetupGet(e => e.IsPaused).Returns(true);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.Pause(), Times.Never);
        }

        [TestMethod]
        public void InitiateCallsStartReplayOnHost()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(true);
            hostMock.SetupGet(e => e.IsPaused).Returns(false);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.StartReplay());
        }

        [TestMethod]
        public void InitiateCallsStopReplayOnHost()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(true);
            hostMock.SetupGet(e => e.IsPaused).Returns(false);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.StopReplay());
        }

        [TestMethod]
        public void InitiateCallsResumeOnHostIfListening()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(true);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.Resume());
        }

        [TestMethod]
        public void InitiateDoesNotCallResumeIfNotListening()
        {
            // Arrange
            Mock<IMicroserviceReplayHost> hostMock = new Mock<IMicroserviceReplayHost>();
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IReplayCommandPublisher commandPublisher = new ReplayCommandPublisher(hostMock.Object, commandPublisherMock.Object);

            hostMock.SetupGet(e => e.IsListening).Returns(false);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            commandPublisher.Initiate(command);

            // Assert
            hostMock.Verify(e => e.Resume(), Times.Never);
        }
    }
}
