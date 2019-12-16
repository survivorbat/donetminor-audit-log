using System.Collections.Generic;
using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Commands;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Unit.Models
{
    [TestClass]
    public class AuditLogitemCriteriaTest
    {
        [TestMethod]
        [DataRow("TestTopic,TopicTest", "TestType", 10392, 29492742)]
        [DataRow("Sometopic", "sometype", 11340392, 2924492742)]
        public void CriteriaCanBeCreatedFromCastingReplayCommand(string topics, string types, long fromTimestamp,
            long toTimeStamp)
        {
            // Arrange
            List<string> topicNames = topics.Split(',').ToList();
            List<string> typeNames = types.Split(',').ToList();

            ReplayEventsCommand replayEventsCommand = new ReplayEventsCommand(toTimeStamp)
            {
                FromTimeStamp = fromTimestamp,
                Topics = topicNames,
                Types = typeNames
            };

            // Act
            AuditLogItemCriteria criteria = (AuditLogItemCriteria) replayEventsCommand;

            // Assert
            Assert.AreEqual(topicNames, criteria.Topics);
            Assert.AreEqual(typeNames, criteria.Types);
            Assert.AreEqual(fromTimestamp, criteria.FromTimeStamp);
            Assert.AreEqual(toTimeStamp, criteria.ToTimeStamp);
        }
    }
}
