using System;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.Extensions.Logging;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Repositories
{
    public class AuditLogItemRepository : IAuditLogItemRepository
    {
        /// <summary>
        /// Context
        /// </summary>
        private readonly AuditLogContext _auditLogContext;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<AuditLogItemRepository> _logger;

        /// <summary>
        /// Inject needed dependencies
        /// </summary>
        public AuditLogItemRepository(AuditLogContext context, ILoggerFactory loggerFactory)
        {
            _auditLogContext = context;
            _logger = loggerFactory.CreateLogger<AuditLogItemRepository>();
        }

        /// <summary>
        /// Save a log item to the database
        /// </summary>
        public void Save(AuditLogItem item)
        {
            _logger.LogDebug($"Saving auditlogitem with ID {item.Id}, " +
                                   $"Topic {item.Topic}, timestamp {item.TimeStamp} " +
                                   $"and data {item.StringData}");

            _auditLogContext.AuditLogItems.Add(item);
            int rowChanges = _auditLogContext.SaveChanges();

            _logger.LogInformation($"{rowChanges} changes made in database after saving item" +
                                   item.Id);
        }
    }
}
