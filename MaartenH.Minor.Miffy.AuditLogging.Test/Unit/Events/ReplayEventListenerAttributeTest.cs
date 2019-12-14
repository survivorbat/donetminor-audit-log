using MaartenH.Minor.Miffy.AuditLogging.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Events
{
    [TestClass]
    public class ReplayEventListenerAttributeTest
    {
        [TestMethod]
        [DataRow("random-queue")]
        [DataRow("queue.name")]
        public void QueueNameIsProperlySet(string queueName)
        {
            // Act
            ReplayEventListenerAttribute attribute = new ReplayEventListenerAttribute(queueName);

            // Assert
            Assert.AreEqual(queueName, attribute.QueueName);
        }
    }
}
