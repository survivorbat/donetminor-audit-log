using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MaartenH.Minor.Miffy.AuditLogging.Commands;
using MaartenH.Minor.Miffy.AuditLogging.Constants;
using MaartenH.Minor.Miffy.AuditLogging.Events;
using MaartenH.Minor.Miffy.AuditLogging.Server.Abstract;
using MaartenH.Minor.Miffy.AuditLogging.Server.Models;
using Microsoft.Extensions.Logging;
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
        /// Logger
        /// </summary>
        private readonly ILogger<ReplayCommandListener> _logger;

        /// <summary>
        /// Instantiate a replay command listener
        /// </summary>
        public ReplayCommandListener(IAuditLogItemRepository repository, IEventPublisher eventPublisher, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _logger = loggerFactory.CreateLogger<ReplayCommandListener>();
        }

        /// <summary>
        /// Start spamming events to the designated queues
        /// </summary>
        [CommandListener(ReplayTopicNames.ReplayEventsCommandDestinationQueue)]
        public ReplayEventsResult Handle(ReplayEventsCommand command)
        {
            AuditLogItemCriteria criteria = (AuditLogItemCriteria) command;

            _logger.LogInformation($"Received replaycommand with criteria: From: {criteria.FromTimeStamp}, " +
                                   $"to: {criteria.ToTimeStamp}, " +
                                   $"topics: {string.Join(',', criteria.Topics)} and " +
                                   $"types: {string.Join(',', criteria.Types)}");

            IEnumerable<AuditLogItem> auditLogItems = _repository.FindBy(criteria).ToList();

            _logger.LogDebug($"Found {auditLogItems.Count()} in the database");

            Task.Run(() =>
            {
                _logger.LogInformation($"Publishing start event with process id {command.ProcessId}");
                _eventPublisher.Publish(new StartReplayEvent(command.ProcessId));

                List<Task> tasks = new List<Task>();
                foreach (AuditLogItem logItem in auditLogItems)
                {
                    _logger.LogTrace($"Publishing logitem with id {logItem.Id}");

                    var task =_eventPublisher.PublishAsync(logItem.TimeStamp,
                        $"{ReplayTopicNames.ReplayEventTopicPrefix}{logItem.Topic}", new Guid(logItem.Id), logItem.Type,
                        logItem.Data);

                    tasks.Add(task);
                };

                _logger.LogTrace("Waiting for all events to be published");
                Task.WaitAll(tasks.ToArray());

                _logger.LogInformation($"Publishing end event with process id {command.ProcessId}");
                _eventPublisher.Publish(new EndReplayEvent(command.ProcessId));
            });

            _logger.LogDebug("Publishing ReplayEventsResult with auditlogitems count");
            return new ReplayEventsResult { AmountOfEvents = auditLogItems.Count() };
        }
    }
}
