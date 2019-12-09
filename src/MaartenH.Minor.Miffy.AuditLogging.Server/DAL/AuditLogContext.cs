using System;
using MaartenH.Minor.Miffy.AuditLogging.Server.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.DAL
{
    public class AuditLogContext : DbContext
    {
		public AuditLogContext()
		{
		}

        public AuditLogContext(DbContextOptions<AuditLogContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    Environment.GetEnvironmentVariable(EnvNames.DatabaseConnectionString));
            }
        }

        internal DbSet<AuditLogItem> AuditLogItems { get; set; }
    }
}
