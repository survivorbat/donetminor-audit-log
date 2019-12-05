using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.DAL
{
    public class AuditLogContext : DbContext
    {
        public AuditLogContext(DbContextOptions<AuditLogContext> options) : base(options)
        {

        }

        public DbSet<AuditLogItem> AuditLogItems { get; set; }
    }
}
