using System;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.Extensions.Logging;
using Minor.Miffy.MicroServices.Events;
using Newtonsoft.Json;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners
{
    /// <summary>
    /// Fan-in event listener that listens to all events coming in
    /// </summary>
    public class AuditEventLoggingListener
    {
        /// <summary>
        /// Repository used to save items with
        /// </summary>
        private readonly IAuditLogItemRepository _repository;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<AuditEventLoggingListener> _logger;

        /// <summary>
        /// Instantiate an auditlog event listener with a repository and a loggerfactory
        /// </summary>
        public AuditEventLoggingListener(IAuditLogItemRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<AuditEventLoggingListener>();
        }

        /// <summary>
        /// Handle an incoming event
        /// </summary>
        [EventListener("auditlog.events")]
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
