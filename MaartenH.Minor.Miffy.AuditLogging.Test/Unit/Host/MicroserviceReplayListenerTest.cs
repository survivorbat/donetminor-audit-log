using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Host
{
    [TestClass]
    public class MicroserviceReplayListenerTest
    {
        [TestMethod]
        [DataRow("test.queue", "random,topic,list")]
        [DataRow("some-secret-queue", "random,secret")]
        public void EventListenerSettingsAreProperlyAppliedToReplyListener(string queueName, string topics)
        {
            // Arrange
            string[] topicNames = topics.Split(",");

            MicroserviceListener listener = new MicroserviceListener { Queue = queueName, TopicExpressions = topicNames };

            MicroserviceReplayListener replayListener = new MicroserviceReplayListener();

            // Act
            replayListener.ApplyListenerSettings(listener);

            // Assert
            string[] expectedTopicNames = topicNames.Select(e => $"{ReplayTopicNames.ReplayEventTopicPrefix}{e}").ToArray();

            Assert.AreEqual($"{ReplayTopicNames.ReplayEventTopicPrefix}{queueName}", replayListener.Queue);
            CollectionAssert.AreEqual(expectedTopicNames, replayListener.TopicExpressions.ToArray());
        }
    }
}
