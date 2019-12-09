using System.Diagnostics.CodeAnalysis;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Models
{
    [ExcludeFromCodeCoverage]
    public class AuditLogItem
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public long TimeStamp { get; set; }
        public string Data { get; set; }
    }
}
