namespace MaartenH.Minor.Miffy.AuditLogging.Constants
{
    public static class ReplayTopicNames
    {
        public const string ReplayStartEventTopic = "auditlog.replay.start";
        public const string ReplayEndEventTopic = "auditlog.replay.end";
        public const string ReplayEventsCommandDestinationQueue = "auditlog.replay";
        public const string ReplayEventTopicPrefix = "replay_";
    }
}
