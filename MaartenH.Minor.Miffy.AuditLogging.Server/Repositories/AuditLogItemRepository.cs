using System.Collections.Generic;
using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Repositories
{
    /// <summary>
    /// EntityFramework implementation of the auditlogger
    /// </summary>
    public class AuditLogItemRepository : IAuditLogItemRepository
    {
        /// <summary>
        /// Context
        /// </summary>
        private readonly AuditLogContext _auditLogContext;

        /// <summary>
        /// Inject needed dependencies
        /// </summary>
        public AuditLogItemRepository(AuditLogContext context)
        {
            _auditLogContext = context;
        }

        /// <summary>
        /// Save a log item to the database
        /// </summary>
        public void Save(AuditLogItem item)
        {
            _auditLogContext.AuditLogItems.Add(item);
            _auditLogContext.SaveChanges();
        }

        /// <summary>
        /// Retrieve audit log items based on criteria
        /// </summary>
        public IEnumerable<AuditLogItem> FindBy(AuditLogItemCriteria criteria)
        {
            return _auditLogContext.AuditLogItems
                .AsNoTracking()
                .AsParallel()
                .Where(dbItem => criteria.AllowMetaEvents || !ReplayTopicNames.MetaTopics.Contains(dbItem.Topic))
                .Where(dbItem => criteria.ToTimeStamp >= dbItem.TimeStamp)
                .Where(dbItem => criteria.FromTimeStamp <= dbItem.TimeStamp)
                .Where(dbItem => !criteria.Topics.Any() || criteria.Topics.Contains(dbItem.Topic))
                .Where(dbItem => !criteria.Types.Any() || criteria.Types.Contains(dbItem.Type));
        }
    }
}
