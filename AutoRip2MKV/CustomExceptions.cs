using System;

namespace AutoRip2MKV
{
    /// <summary>
    /// Base exception for all AutoRip2MKV specific exceptions
    /// </summary>
    public abstract class AutoRip2MKVException : Exception
    {
        protected AutoRip2MKVException() { }
        protected AutoRip2MKVException(string message) : base(message) { }
        protected AutoRip2MKVException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when configuration validation fails
    /// </summary>
    public class ConfigurationException : AutoRip2MKVException
    {
        public ConfigurationException() { }
        public ConfigurationException(string message) : base(message) { }
        public ConfigurationException(string message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// Thrown when MakeMKV operations fail
    /// </summary>
    public class MakeMKVException : AutoRip2MKVException
    {
        public int ExitCode { get; }

        public MakeMKVException() { }
        public MakeMKVException(string message) : base(message) { }
        public MakeMKVException(string message, Exception innerException) : base(message, innerException) { }
        public MakeMKVException(string message, int exitCode) : base(message)
        {
            ExitCode = exitCode;
        }
        public MakeMKVException(string message, int exitCode, Exception innerException) : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }

    /// <summary>
    /// Thrown when HandBrake operations fail
    /// </summary>
    public class HandBrakeException : AutoRip2MKVException
    {
        public int ExitCode { get; }

        public HandBrakeException() { }
        public HandBrakeException(string message) : base(message) { }
        public HandBrakeException(string message, Exception innerException) : base(message, innerException) { }
        public HandBrakeException(string message, int exitCode) : base(message)
        {
            ExitCode = exitCode;
        }
        public HandBrakeException(string message, int exitCode, Exception innerException) : base(message, innerException)
        {
            ExitCode = exitCode;
        }
    }

    /// <summary>
    /// Thrown when disc operations fail
    /// </summary>
    public class DiscException : AutoRip2MKVException
    {
        public string DriveLetter { get; }

        public DiscException() { }
        public DiscException(string message) : base(message) { }
        public DiscException(string message, Exception innerException) : base(message, innerException) { }
        public DiscException(string message, string driveLetter) : base(message)
        {
            DriveLetter = driveLetter;
        }
        public DiscException(string message, string driveLetter, Exception innerException) : base(message, innerException)
        {
            DriveLetter = driveLetter;
        }
    }

    /// <summary>
    /// Thrown when credential operations fail
    /// </summary>
    public class CredentialException : AutoRip2MKVException
    {
        public string CredentialKey { get; }

        public CredentialException() { }
        public CredentialException(string message) : base(message) { }
        public CredentialException(string message, Exception innerException) : base(message, innerException) { }
        public CredentialException(string message, string credentialKey) : base(message)
        {
            CredentialKey = credentialKey;
        }
        public CredentialException(string message, string credentialKey, Exception innerException) : base(message, innerException)
        {
            CredentialKey = credentialKey;
        }
    }

    /// <summary>
    /// Thrown when email/notification operations fail
    /// </summary>
    public class NotificationException : AutoRip2MKVException
    {
        public string NotificationType { get; }

        public NotificationException() { }
        public NotificationException(string message) : base(message) { }
        public NotificationException(string message, Exception innerException) : base(message, innerException) { }
        public NotificationException(string message, string notificationType) : base(message)
        {
            NotificationType = notificationType;
        }
        public NotificationException(string message, string notificationType, Exception innerException) : base(message, innerException)
        {
            NotificationType = notificationType;
        }
    }

    /// <summary>
    /// Thrown when file operations fail
    /// </summary>
    public class FileOperationException : AutoRip2MKVException
    {
        public string FilePath { get; }
        public string Operation { get; }

        public FileOperationException() { }
        public FileOperationException(string message) : base(message) { }
        public FileOperationException(string message, Exception innerException) : base(message, innerException) { }
        public FileOperationException(string message, string operation, string filePath) : base(message)
        {
            Operation = operation;
            FilePath = filePath;
        }
        public FileOperationException(string message, string operation, string filePath, Exception innerException) : base(message, innerException)
        {
            Operation = operation;
            FilePath = filePath;
        }
    }
}
