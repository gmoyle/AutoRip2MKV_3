using System;
using System.IO;
using AutoRip2MKV.Properties;

namespace AutoRip2MKV
{
    public interface IConfigurationManager
    {
        ValidationResult ValidateAndInitialize();
        void CreateMissingDirectories();
        void SetDefaultValues();
        string GetTempPath();
        string GetFinalPath();
        string GetCurrentTitle();
        void UpdateCurrentTitle(string title);
        void SaveConfiguration();
        bool IsConfigurationValid();
    }

    public class ConfigurationManager : IConfigurationManager
    {
        private readonly ILogger _logger;
        private readonly IConfigurationValidator _validator;
        private readonly IFileOperations _fileOperations;

        public ConfigurationManager(ILogger logger, IConfigurationValidator validator, IFileOperations fileOperations)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
        }

        public ValidationResult ValidateAndInitialize()
        {
            _logger.Info("Starting configuration validation and initialization");

            // Set default values if needed
            SetDefaultValues();

            // Validate configuration
            var validationResult = _validator.ValidateConfiguration();

            // Create missing directories if validation passed
            if (validationResult.IsValid)
            {
                try
                {
                    CreateMissingDirectories();
                    _logger.Info("Configuration validation and initialization completed successfully");
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to create missing directories during initialization");
                    validationResult.AddError($"Failed to create directories: {ex.Message}");
                }
            }
            else
            {
                _logger.Warn("Configuration validation failed. Errors: {0}", string.Join("; ", validationResult.Errors));
            }

            // Log warnings if any
            if (validationResult.Warnings.Count > 0)
            {
                _logger.Warn("Configuration warnings: {0}", string.Join("; ", validationResult.Warnings));
            }

            return validationResult;
        }

        public void CreateMissingDirectories()
        {
            _logger.Info("Creating missing directories");

            var tempPath = GetTempPath();
            var finalPath = GetFinalPath();

            if (!string.IsNullOrWhiteSpace(tempPath))
            {
                try
                {
                    var createTempTask = _fileOperations.CreateDirectoryAsync(tempPath);
                    createTempTask.Wait();
                    _logger.Info("Ensured temp directory exists: {0}", tempPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to create temp directory: {0}", tempPath);
                    throw;
                }
            }

            if (!string.IsNullOrWhiteSpace(finalPath))
            {
                try
                {
                    var createFinalTask = _fileOperations.CreateDirectoryAsync(finalPath);
                    createFinalTask.Wait();
                    _logger.Info("Ensured final directory exists: {0}", finalPath);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Failed to create final directory: {0}", finalPath);
                    throw;
                }
            }
        }

        public void SetDefaultValues()
        {
            _logger.Debug("Setting default configuration values where needed");

            // Set default temp path if not configured
            if (string.IsNullOrWhiteSpace(Settings.Default.TempPath))
            {
                var defaultTempPath = Path.Combine(Path.GetTempPath(), "AutoRip2MKV", "Temp");
                Settings.Default.TempPath = defaultTempPath;
                _logger.Info("Set default temp path: {0}", defaultTempPath);
            }

            // Set default minimum title length if not configured
            if (string.IsNullOrWhiteSpace(Settings.Default.MinTitleLength))
            {
                Settings.Default.MinTitleLength = "20"; // 20 minutes default
                _logger.Info("Set default minimum title length: 20 minutes");
            }

            // Set default SMTP port if not configured
            if (Settings.Default.SMTPPort <= 0)
            {
                Settings.Default.SMTPPort = 587; // Standard TLS port
                _logger.Info("Set default SMTP port: 587");
            }

            // Set default handbrake parameters if conversion is enabled but parameters are empty
            if (Settings.Default.ConvWithHandbrake && string.IsNullOrWhiteSpace(Settings.Default.HandBrakeParameters))
            {
                Settings.Default.HandBrakeParameters = ".mp4 -e x264 -q 20 -B 160";
                _logger.Info("Set default Handbrake parameters: {0}", Settings.Default.HandBrakeParameters);
            }

            // Enable timeout by default if not set
            if (!Settings.Default.HasSetTimeout) // Assume this property exists or add it
            {
                Settings.Default.Timeout = true;
                Settings.Default.TimerValue = 30; // 30 seconds default
                _logger.Info("Set default timeout: enabled, 30 seconds");
            }

            SaveConfiguration();
        }

        public string GetTempPath()
        {
            var tempPath = Settings.Default.TempPath;
            if (string.IsNullOrWhiteSpace(tempPath))
            {
                tempPath = Path.Combine(Path.GetTempPath(), "AutoRip2MKV", "Temp");
                _logger.Debug("Using fallback temp path: {0}", tempPath);
            }
            return tempPath;
        }

        public string GetFinalPath()
        {
            return Settings.Default.FinalPath ?? string.Empty;
        }

        public string GetCurrentTitle()
        {
            return Settings.Default.CurrentTitle ?? string.Empty;
        }

        public void UpdateCurrentTitle(string title)
        {
            if (title != Settings.Default.CurrentTitle)
            {
                _logger.Info("Updating current title from '{0}' to '{1}'", Settings.Default.CurrentTitle, title);
                Settings.Default.CurrentTitle = title;
                SaveConfiguration();
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                Settings.Default.Save();
                Settings.Default.Reload();
                _logger.Debug("Configuration saved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to save configuration");
                throw;
            }
        }

        public bool IsConfigurationValid()
        {
            try
            {
                var result = _validator.ValidateConfiguration();
                return result.IsValid;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to validate configuration");
                return false;
            }
        }
    }
}
