namespace MaartenH.Minor.Miffy.AuditLogging.Server.Models
{
    public class AuditLogItemCriteria
    {
        public string? EventType { get; set; }
        public string? Topic { get; set; }
        public long? FromTimeStamp { get; set; }
        public long ToTimeStamp { get; set; }
    }
}
