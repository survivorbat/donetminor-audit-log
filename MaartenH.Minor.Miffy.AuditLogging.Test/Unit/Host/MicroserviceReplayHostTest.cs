using System.Collections.Generic;
using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Moq;
using RabbitMQ.Client;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Host
{
    [TestClass]
    public class MicroserviceReplayHostTest
    {
        [TestMethod]
        public void IsReplayingIsStandardFalse()
        {
            // Arrange
            var connectionMock = new Mock<IBusContext<IConnection>>();
            List<MicroserviceListener> listeners = new List<MicroserviceListener>();
            List<MicroserviceReplayListener> replayListeners = new List<MicroserviceReplayListener>();
            List<MicroserviceCommandListener> commandListeners = new List<MicroserviceCommandListener>();

            // Act
            IMicroserviceReplayHost replayHost = new MicroserviceReplayHost(connectionMock.Object, listeners, replayListeners, commandListeners, new NullLogger<MicroserviceHost>());

            // Assert
            Assert.AreEqual(false, replayHost.IsReplaying);
        }

        [TestMethod]
        public void StartReplayThrowsExceptionIfAlreadyReplaying()
        {
            // Arrange
            var connectionMock = new Mock<IBusContext<IConnection>>();
            List<MicroserviceListener> listeners = new List<MicroserviceListener>();
            List<MicroserviceReplayListener> replayListeners = new List<MicroserviceReplayListener>();
            List<MicroserviceCommandListener> commandListeners = new List<MicroserviceCommandListener>();

            IMicroserviceReplayHost replayHost = new MicroserviceReplayHost(connectionMock.Object, listeners, replayListeners, commandListeners, new NullLogger<MicroserviceHost>());

            replayHost.StartReplay();

            // Act
            void Act() => replayHost.StartReplay();

            // Assert
            BusConfigurationException exception = Assert.ThrowsException<BusConfigurationException>(Act);
            Assert.AreEqual("Attempted to replay the MicroserviceHost, but it is already replaying.", exception.Message);
        }

        [TestMethod]
        public void StartReplaySetsIsReplayingToTrue()
        {
            // Arrange
            var connectionMock = new Mock<IBusContext<IConnection>>();
            List<MicroserviceListener> listeners = new List<MicroserviceListener>();
            List<MicroserviceReplayListener> replayListeners = new List<MicroserviceReplayListener>();
            List<MicroserviceCommandListener> commandListeners = new List<MicroserviceCommandListener>();

            IMicroserviceReplayHost replayHost = new MicroserviceReplayHost(connectionMock.Object, listeners, replayListeners, commandListeners, new NullLogger<MicroserviceHost>());

            // Act
            replayHost.StartReplay();

            // Assert
            Assert.AreEqual(true, replayHost.IsReplaying);
        }

        [TestMethod]
        public void StopReplaySetsIsReplayingToFalse()
        {
            // Arrange
            var connectionMock = new Mock<IBusContext<IConnection>>();
            List<MicroserviceListener> listeners = new List<MicroserviceListener>();
            List<MicroserviceReplayListener> replayListeners = new List<MicroserviceReplayListener>();
            List<MicroserviceCommandListener> commandListeners = new List<MicroserviceCommandListener>();

            IMicroserviceReplayHost replayHost = new MicroserviceReplayHost(connectionMock.Object, listeners, replayListeners, commandListeners, new NullLogger<MicroserviceHost>());

            replayHost.StartReplay();

            // Act
            replayHost.StopReplay();

            // Assert
            Assert.AreEqual(false, replayHost.IsReplaying);
        }

        [TestMethod]
        public void StopReplayThrowsExceptionIfNotReplaying()
        {
            // Arrange
            var connectionMock = new Mock<IBusContext<IConnection>>();
            List<MicroserviceListener> listeners = new List<MicroserviceListener>();
            List<MicroserviceReplayListener> replayListeners = new List<MicroserviceReplayListener>();
            List<MicroserviceCommandListener> commandListeners = new List<MicroserviceCommandListener>();

            IMicroserviceReplayHost replayHost = new MicroserviceReplayHost(connectionMock.Object, listeners, replayListeners, commandListeners, new NullLogger<MicroserviceHost>());

            // Act
            void Act() => replayHost.StopReplay();

            // Assert
            BusConfigurationException exception = Assert.ThrowsException<BusConfigurationException>(Act);
            Assert.AreEqual("Attempted to stop replaying the MicroserviceHost, but it is not replaying.", exception.Message);
        }
    }
}
