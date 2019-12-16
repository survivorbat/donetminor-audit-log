using System.Collections.Generic;
using System.Linq;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.DAL;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;

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
                .Where(e => criteria.ToTimeStamp >= e.TimeStamp)
                .Where(e => criteria.FromTimeStamp == null || criteria.FromTimeStamp <= e.TimeStamp)
                .Where(e => criteria.Topics == null || criteria.Topics.Contains(e.Topic))
                .Where(e => criteria.Types == null || criteria.Types.Contains(e.Type));
        }
    }
}
