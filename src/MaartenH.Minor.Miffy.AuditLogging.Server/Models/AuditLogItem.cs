using System;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Models
{
    public class AuditLogItem
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public long TimeStamp { get; set; }
        public byte[] Data { get; set; }
        public string StringData { get; set; }
    }
}
