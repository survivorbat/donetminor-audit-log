using System;
using System.Diagnostics.CodeAnalysis;
using MaartenH.Minor.Miffy.AuditLogging.Server.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.DAL
{
    /// <summary>
    /// Context used to save items to the database
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AuditLogContext : DbContext
    {
        /// <summary>
        /// Empty context for migrations
        /// </summary>
		public AuditLogContext()
		{
		}

        /// <summary>
        /// Context used to boot the application
        /// </summary>
        public AuditLogContext(DbContextOptions<AuditLogContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configure context
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    Environment.GetEnvironmentVariable(EnvVarNames.DatabaseConnectionString));
            }
        }

        /// <summary>
        /// All the auditlog items
        /// </summary>
        public DbSet<AuditLogItem> AuditLogItems { get; set; }
    }
}
