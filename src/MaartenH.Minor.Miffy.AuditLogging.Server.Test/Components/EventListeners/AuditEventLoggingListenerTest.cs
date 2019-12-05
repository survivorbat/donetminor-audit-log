using Blueshift.EntityFrameworkCore.MongoDB.Storage;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners;
using MaartenH.Minor.Miffy.AuditLogging.Server.Repositories;
using MaartenH.Minor.Miffy.AuditLogging.Server.Test.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;
using MongoDB.Driver;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Test.Components.EventListeners
{
    [TestClass]
    public class AuditEventLoggingListenerTest
    {
        private IMongoClient _mongoClient;
        private DbContextOptions<AuditLogContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _mongoClient = new MongoClient();

            // TODO: Finish connection
//            _options = new DbContextOptionsBuilder<AuditLogContext>()
//                .UseMongoDb(_mongoClient)
//                .Options;
        }

        [TestMethod]
        public void EventIsProperlyReceived()
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

            DummyEvent evt = new DummyEvent("test.queue");
            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            // Act
            eventPublisher.Publish(evt);

            // Assert

        }
    }
}
