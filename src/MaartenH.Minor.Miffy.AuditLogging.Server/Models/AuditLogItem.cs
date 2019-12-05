using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.Models
{
    public class AuditLogItem
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}
