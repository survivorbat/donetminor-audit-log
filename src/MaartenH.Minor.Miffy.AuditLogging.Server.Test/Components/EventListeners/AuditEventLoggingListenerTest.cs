using System.Linq;
using System.Threading;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using MaartenH.Minor.Miffy.AuditLogging.Server.Repositories;
using MaartenH.Minor.Miffy.AuditLogging.Server.Test.Events;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;
using Newtonsoft.Json;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Components.EventListeners
{
    [TestClass]
    public class AuditEventLoggingListenerTest
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
        [DataRow("Very Secret Message")]
        [DataRow("A new user was added to the system!")]
        [DataRow("Hello World!")]
        public void EventIsProperlyReceived(string data)
        {
            // Arrange
            AuditLogContext dbContext = new AuditLogContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            using var hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddTransient<IAuditLogItemRepository, AuditLogItemRepository>();
                })
                .AddEventListener<IAuditLogItemRepository>()
                .AddEventListener<AuditEventLoggingListener>();

            using var host = hostBuilder.CreateHost();

            host.Start();

            DummyEvent evt = new DummyEvent("test.queue") {Text = data };

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            // Act
            eventPublisher.Publish(evt);

            Thread.Sleep(1500);

            // Assert
            AuditLogItem[] resultData = dbContext.AuditLogItems.ToArray();
            Assert.AreEqual(1, resultData.Length);

            string expectedData = JsonConvert.SerializeObject(evt);

            var firstItem = resultData.First();
            Assert.AreEqual(expectedData, firstItem.Data);
            Assert.AreEqual(evt.Id.ToString(), firstItem.Id);
            Assert.AreEqual(evt.Timestamp, firstItem.TimeStamp);
        }
    }
}
