using System;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Models
{
    public class AuditLogItem
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}
