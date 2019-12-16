using System.Collections.Generic;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Abstract
{
    /// <summary>
    /// Repository to save
    /// </summary>
    public interface IAuditLogItemRepository
    {
        /// <summary>
        /// Save an auditlog item
        /// </summary>
        void Save(AuditLogItem item);

        /// <summary>
        /// Find logitems by a specific criteria
        /// </summary>
        IEnumerable<AuditLogItem> FindBy(AuditLogItemCriteria criteria);
    }
}
