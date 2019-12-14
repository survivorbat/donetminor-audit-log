using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Host;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Test.Unit.Host
{
    [TestClass]
    public class MicroserviceReplayListenerTest
    {
        [TestMethod]
        [DataRow("test.queue", "test.queue", true)]
        [DataRow("some-queue", "some-queue", true)]
        [DataRow("test.queue", "some-queue", false)]
        [DataRow("some-queue", "test.queue", false)]
        public void EventListenerAndReplayListenerWithTheSameQueueNameAreEqual(string queueNameOne, string queueNameTwo, bool expected)
        {
            // Arrange
            MicroserviceListener listener = new MicroserviceListener { Queue = queueNameOne };
            MicroserviceReplayListener replayListener = new MicroserviceReplayListener { Queue = queueNameTwo };

            // Act
            bool result = replayListener.Equals(listener);

            // Assert
            Assert.AreEqual(expected, result);
        }

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
            string[] expectedTopicNames = topicNames.Select(e => $"replay_{e}").ToArray();

            Assert.AreEqual($"replay_{queueName}", replayListener.Queue);
            CollectionAssert.AreEqual(expectedTopicNames, replayListener.TopicExpressions.ToArray());
        }
    }
}
