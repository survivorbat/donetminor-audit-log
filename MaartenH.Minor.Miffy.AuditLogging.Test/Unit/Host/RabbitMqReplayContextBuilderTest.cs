using System;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Host
{
    [TestClass]
    public class RabbitMqReplayContextBuilderTest
    {
        [TestMethod]
        [DataRow("Test-Exchange")]
        [DataRow("secret.exchange")]
        public void WithReplayExchangePrefixSetsPrefix(string exchangePrefix)
        {
            // Arrange
            RabbitMqReplayContextBuilder contextBuilder = new RabbitMqReplayContextBuilder();

            // Act
            contextBuilder.WithReplayExchangePrefix(exchangePrefix);

            // Assert
            Assert.AreEqual(exchangePrefix, contextBuilder.ReplayExchangePrefix);
        }

        [TestMethod]
        public void ReadEnvironmentVariablesThrowsExceptionOnUnsetExchangePrefix()
        {
            // Arrange
            RabbitMqReplayContextBuilder contextBuilder = new RabbitMqReplayContextBuilder();

            Environment.SetEnvironmentVariable(EnvVarNames.ReplayExchangeName, null);
            Environment.SetEnvironmentVariable("BROKER_CONNECTION_STRING", "");
            Environment.SetEnvironmentVariable("BROKER_EXCHANGE_NAME", "");

            // Act
            void Act() => contextBuilder.ReadFromEnvironmentVariables();

            // Assert
            BusConfigurationException exception = Assert.ThrowsException<BusConfigurationException>(Act);
            Assert.AreEqual($"{EnvVarNames.ReplayExchangeName} variable not set", exception.Message);
        }

        [TestMethod]
        [DataRow("Test-Exchange")]
        [DataRow("secret.exchange")]
        public void ReadEnvironmentVariablesReadsExchangePrefixProperly(string exchangePrefix)
        {
            // Arrange
            RabbitMqReplayContextBuilder contextBuilder = new RabbitMqReplayContextBuilder();

            Environment.SetEnvironmentVariable(EnvVarNames.ReplayExchangeName, exchangePrefix);
            Environment.SetEnvironmentVariable("BROKER_CONNECTION_STRING", "amqp://guest:guest@localhost");
            Environment.SetEnvironmentVariable("BROKER_EXCHANGE_NAME", "Test");

            // Act
            contextBuilder.ReadFromEnvironmentVariables();

            // Assert
            Assert.AreEqual(exchangePrefix, contextBuilder.ReplayExchangePrefix);
        }
    }
}
