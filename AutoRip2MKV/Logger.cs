using System;
using NLog;

namespace AutoRip2MKV
{
    /// <summary>
    /// Centralized logging infrastructure using NLog
    /// </summary>
    public static class Logger
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        static Logger()
        {
            // Configure NLog programmatically if no config file exists
            if (LogManager.Configuration == null)
            {
                var config = new NLog.Config.LoggingConfiguration();
                
                // Create targets
                var fileTarget = new NLog.Targets.FileTarget("fileTarget")
                {
                    FileName = "${basedir}/logs/AutoRip2MKV-${shortdate}.log",
                    Layout = "${longdate} ${level:uppercase=true} ${logger} ${message} ${exception:format=tostring}",
                    ArchiveEvery = NLog.Targets.FileArchivePeriod.Day,
                    ArchiveNumbering = NLog.Targets.ArchiveNumberingMode.Rolling,
                    MaxArchiveFiles = 7
                };

                var consoleTarget = new NLog.Targets.ColoredConsoleTarget("consoleTarget")
                {
                    Layout = "${time} [${level:uppercase=true}] ${message} ${exception:format=tostring}"
                };

                // Add targets to configuration
                config.AddTarget(fileTarget);
                config.AddTarget(consoleTarget);

                // Define rules
                config.AddRule(LogLevel.Debug, LogLevel.Fatal, fileTarget);
                config.AddRule(LogLevel.Info, LogLevel.Fatal, consoleTarget);

                // Apply configuration
                LogManager.Configuration = config;
            }
        }

        public static void Debug(string message, params object[] args)
        {
            _logger.Debug(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            _logger.Info(message, args);
        }

        public static void Warn(string message, params object[] args)
        {
            _logger.Warn(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            _logger.Error(message, args);
        }

        public static void Error(Exception exception, string message, params object[] args)
        {
            _logger.Error(exception, message, args);
        }

        public static void Fatal(string message, params object[] args)
        {
            _logger.Fatal(message, args);
        }

        public static void Fatal(Exception exception, string message, params object[] args)
        {
            _logger.Fatal(exception, message, args);
        }

        /// <summary>
        /// Logs operation start for tracking
        /// </summary>
        public static void LogOperationStart(string operation, params object[] parameters)
        {
            Info("Starting operation: {0} with parameters: {1}", operation, string.Join(", ", parameters));
        }

        /// <summary>
        /// Logs operation completion for tracking
        /// </summary>
        public static void LogOperationComplete(string operation, TimeSpan duration)
        {
            Info("Completed operation: {0} in {1}", operation, duration);
        }

        /// <summary>
        /// Logs operation failure for tracking
        /// </summary>
        public static void LogOperationFailure(string operation, Exception exception)
        {
            Error(exception, "Failed operation: {0}", operation);
        }
    }
}
