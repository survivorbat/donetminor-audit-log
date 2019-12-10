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
            // Arrange
            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            string destinationQueue = command.DestinationQueue;

            // Assert
            Assert.AreEqual("auditlog.replay", destinationQueue);
        }
    }
}
