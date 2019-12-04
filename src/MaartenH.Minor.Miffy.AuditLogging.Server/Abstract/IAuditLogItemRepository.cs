using MaartenH.Minor.Miffy.AuditLogging.Server.Models;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Abstract
{
    public interface IAuditLogItemRepository
    {
        void Save(AuditLogItem item);
    }
}
