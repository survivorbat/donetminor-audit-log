using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaartenH.Minor.Miffy.AuditLogging.Commands;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Events;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Minor.Miffy.MicroServices.Events;

namespace MaartenH.Minor.Miffy.AuditLogging.Server.CommandListeners
{
    /// <summary>
    /// Listener that waits for commands to start blasting events to the bus
    /// </summary>
    public class ReplayCommandListener
    {
        /// <summary>
        /// Repository
        /// </summary>
        private readonly IAuditLogItemRepository _repository;

        /// <summary>
        /// Event Publisher
        /// </summary>
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Instantiate a replay command listener
        /// </summary>
        public ReplayCommandListener(IAuditLogItemRepository repository, IEventPublisher eventPublisher)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Start spamming events to the designated queues
        /// </summary>
        [CommandListener(ReplayTopicNames.ReplayEventsCommandDestinationQueue)]
        public ReplayEventsResult Handle(ReplayEventsCommand command)
        {
            AuditLogItemCriteria criteria = (AuditLogItemCriteria) command;
            IEnumerable<AuditLogItem> auditLogItems = _repository.FindBy(criteria);

            Task.Run(() =>
            {
                _eventPublisher.Publish(new StartReplayEvent(command.ProcessId));

                foreach (AuditLogItem logItem in auditLogItems)
                {
                    _eventPublisher.Publish(logItem.TimeStamp,
                        $"{ReplayTopicNames.ReplayEventTopicPrefix}{logItem.Topic}", logItem.Id, logItem.Type,
                        logItem.Data);
                }

                _eventPublisher.Publish(new EndReplayEvent(command.ProcessId));
            });

            return new ReplayEventsResult { AmountOfEvents = auditLogItems.Count() };
        }
    }
}
