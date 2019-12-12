using System;
using MaartenH.Minor.Miffy.AuditLogging.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Commands
{
    [TestClass]
    public class ReplayEventCommandTest
    {
        [TestMethod]
        public void EnsureDestinationQueueIsProperlySet()
        {
            // Arct
            ReplayEventsCommand command = new ReplayEventsCommand(new Guid());

            // Assert
            Assert.AreEqual("auditlog.replay", command.DestinationQueue);
        }

        [TestMethod]
        [DataRow("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC")]
        [DataRow("FAE04EC0-301F-11D3-BF4B-00C04E79EFBC")]
        public void EnsureProcessIdIsProperlySet(string guidString)
        {
            // Arrange
            Guid guid = Guid.Parse(guidString);

            // Act
            ReplayEventsCommand command = new ReplayEventsCommand(guid);

            // Assert
            Assert.AreEqual(guid, command.ProcessId);
        }
    }
}
