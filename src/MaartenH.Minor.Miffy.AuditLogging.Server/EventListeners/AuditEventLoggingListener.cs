using System;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.EventListeners
{
    public class AuditEventLoggingListener
    {
        private readonly IAuditLogItemRepository _repository;

        public AuditEventLoggingListener(IAuditLogItemRepository repository)
        {
            _repository = repository;
        }

        [EventListener("audit.event.queue")]
        [Topic("#")]
        public void Handle(string evt)
        {
            AuditLogItem item = new AuditLogItem
            {
                Data = evt,
                DateTime = DateTime.Now
            };

            _repository.Save(item);
        }
    }
}
