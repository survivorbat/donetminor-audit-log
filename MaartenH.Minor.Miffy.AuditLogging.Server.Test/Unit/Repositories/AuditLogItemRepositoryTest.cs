using System;
using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using MaartenH.Minor.Miffy.AuditLogging.Server.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Unit.Repositories
{
    [TestClass]
    public class AuditLogItemRepositoryTest
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<AuditLogContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<AuditLogContext>()
                .UseSqlite(_connection).Options;

            using var context = new AuditLogContext(_options);
            context.Database.EnsureCreated();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var context = new AuditLogContext(_options);
            context.Set<AuditLogItem>().RemoveRange(context.Set<AuditLogItem>());
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("TestData", "TeestTopic")]
        [DataRow("{ 'data': 'test' }", "Topic.Topic")]
        public void SavingAnItemWorks(string data, string topic)
        {
            // Arrange
            using (var context = new AuditLogContext(_options))
            {
                var repository = new AuditLogItemRepository(context);

                var item = new AuditLogItem
                {
                    Data = data,
                    Topic = topic,
                    Id = Guid.NewGuid().ToString()
                };

                // Act
                repository.Save(item);
            }

            // Assert
            using var controlContext = new AuditLogContext(_options);

            var resultData = controlContext.AuditLogItems.ToArray();

            Assert.AreEqual(1, resultData.Length);

            var firstItem = resultData.First();
            Assert.AreEqual(data, firstItem.Data);
            Assert.AreEqual(topic, firstItem.Topic);
        }

        private readonly AuditLogItem[] _dummyData = {
            new AuditLogItem {TimeStamp = 9, Data = "Test", Topic = "TestTopic", Type = "TestType", Id = Guid.NewGuid().ToString()},
            new AuditLogItem {TimeStamp = 12, Data = "Test", Topic = "TopicTest", Type = "TypeTest", Id = Guid.NewGuid().ToString()},
            new AuditLogItem {TimeStamp = 15, Data = "Test", Topic = "TestTopic", Type = "TestType", Id = Guid.NewGuid().ToString()},
            new AuditLogItem {TimeStamp = 23, Data = "Test", Topic = "TopicTest2", Type = "TypeTest", Id = Guid.NewGuid().ToString()},
            new AuditLogItem {TimeStamp = 120, Data = "Test", Topic = ReplayTopicNames.ReplayStartEventTopic, Type = "EndReplayEvent", Id = Guid.NewGuid().ToString()},
            new AuditLogItem {TimeStamp = 150, Data = "Test", Topic = ReplayTopicNames.ReplayEndEventTopic, Type = "StartReplayEvent", Id = Guid.NewGuid().ToString()},
        };

        [TestMethod]
        [DataRow("TestTopic", 2)]
        [DataRow("TopicTest", 1)]
        [DataRow("TopicTest2", 1)]
        [DataRow("TestTopic,TypeTest", 2)]
        [DataRow("TestTopic,TopicTest2", 3)]
        [DataRow("TestTopic,TopicTest2,TopicTest", 4)]
        public void RetrievingItemsWithTopicWorks(string topics, int expectedAmount)
        {
            string[] topicNames = topics.Split(',');

            // Arrange
            InjectData(_dummyData);

            using var context = new AuditLogContext(_options);
            var repository = new AuditLogItemRepository(context);

            var criteria = new AuditLogItemCriteria { Topics = topicNames, ToTimeStamp = 100 };

            // Act
            AuditLogItem[] results = repository.FindBy(criteria).ToArray();

            // Assert
            Assert.AreEqual(expectedAmount, results.Length);
        }

        [TestMethod]
        [DataRow("TestType", 2)]
        [DataRow("TypeTest", 2)]
        [DataRow("TypeTest,TestType", 4)]
        public void RetrievingItemsWithTypeWorks(string types, int expectedAmount)
        {
            string[] typeNames = types.Split(',');

            // Arrange
            InjectData(_dummyData);

            using var context = new AuditLogContext(_options);
            var repository = new AuditLogItemRepository(context);

            var criteria = new AuditLogItemCriteria { Types = typeNames, ToTimeStamp = 100 };

            // Act
            AuditLogItem[] results = repository.FindBy(criteria).ToArray();

            // Assert
            Assert.AreEqual(expectedAmount, results.Length);
        }

        [TestMethod]
        [DataRow(10, 20, 2)]
        [DataRow(5, 12, 2)]
        [DataRow(10, 25, 3)]
        [DataRow(5, 40, 4)]
        [DataRow(0, 8, 0)]
        [DataRow(0, 1, 0)]
        [DataRow(24, 26, 0)]
        public void RetrievingItemsFromSpecificTimePeriodWorks(long fromTimeStamp, long toTimeStamp, int expectedAmount)
        {
            // Arrange
            InjectData(_dummyData);

            using var context = new AuditLogContext(_options);
            var repository = new AuditLogItemRepository(context);

            var criteria = new AuditLogItemCriteria
            {
                FromTimeStamp = fromTimeStamp,
                ToTimeStamp = toTimeStamp
            };

            // Act
            AuditLogItem[] results = repository.FindBy(criteria).ToArray();

            // Assert
            Assert.AreEqual(expectedAmount, results.Length);
        }

        [TestMethod]
        [DataRow(true, 6)]
        [DataRow(false, 4)]
        public void AllowingMetaEventsFiltersEvents(bool allowMeta, int expectedAmount)
        {
            // Arrange
            InjectData(_dummyData);

            using var context = new AuditLogContext(_options);
            var repository = new AuditLogItemRepository(context);

            var criteria = new AuditLogItemCriteria
            {
                AllowMetaEvents = allowMeta,
                ToTimeStamp = 600,
                FromTimeStamp = 0
            };

            // Act
            AuditLogItem[] results = repository.FindBy(criteria).ToArray();

            // Assert
            Assert.AreEqual(expectedAmount, results.Length);
        }

        /// <summary>
        /// Inject data directly into the context
        /// </summary>
        private void InjectData(params AuditLogItem[] items)
        {
            using var context = new AuditLogContext(_options);
            context.AuditLogItems.AddRange(items);
            context.SaveChanges();
        }
    }
}
