namespace MaartenH.Minor.Miffy.AuditLogging.Server.Constants
{
    /// <summary>
    /// Environment variables used in the application
    /// </summary>
    internal static class EnvVarNames
    {
        /// <summary>
        /// Connection string to the broker
        /// </summary>
        internal const string DatabaseConnectionString = "DB_CONNECTION_STRING";

        /// <summary>
        /// Name of the loglevel currently being used
        /// </summary>
        internal const string LogLevel = "LOG_LEVEL";
    }
}
