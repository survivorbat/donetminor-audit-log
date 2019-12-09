using System;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.Extensions.Logging;
using Minor.Miffy.MicroServices.Events;
using Newtonsoft.Json;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners
{
    public class AuditEventLoggingListener
    {
        private readonly IAuditLogItemRepository _repository;

        private readonly ILogger<AuditEventLoggingListener> _logger;

        public AuditEventLoggingListener(IAuditLogItemRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<AuditEventLoggingListener>();
        }

        [EventListener("audit.event.queue")]
        [Topic("#")]
        public void Handle(string evt)
        {
            _logger.LogDebug($"Deserializing event with data {evt}");

            try
            {
                AuditLogItem item = JsonConvert.DeserializeObject<AuditLogItem>(evt);
                item.Data = evt;

                _repository.Save(item);
            }
            catch (JsonReaderException exception)
            {
                _logger.LogCritical($"Json Exception occured while handling incoming event! {exception.Message}. " +
                                    $"This means that this item IS NOT saved! Item {evt}");

                throw;
            }
        }
    }
}
