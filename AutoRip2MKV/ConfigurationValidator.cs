using System;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AutoRip2MKV.Properties;

namespace AutoRip2MKV
{
    public class ConfigurationValidator : IConfigurationValidator
    {
        private readonly ILogger _logger;
        private readonly IFileOperations _fileOperations;

        public ConfigurationValidator(ILogger logger, IFileOperations fileOperations)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _fileOperations = fileOperations ?? throw new ArgumentNullException(nameof(fileOperations));
        }

        public ValidationResult ValidateConfiguration()
        {
            _logger.Info("Starting comprehensive configuration validation");
            
            var result = new ValidationResult { IsValid = true };

            // Validate all configuration sections
            var pathValidation = ValidateAllPaths();
            var emailValidation = ValidateEmailSettings();
            var handbrakeValidation = ValidateHandbrakeSettings();
            var makemkvValidation = ValidateMakeMKVSettings();

            // Merge all validation results
            MergeValidationResults(result, pathValidation);
            MergeValidationResults(result, emailValidation);
            MergeValidationResults(result, handbrakeValidation);
            MergeValidationResults(result, makemkvValidation);

            _logger.Info("Configuration validation completed. Valid: {0}, Errors: {1}, Warnings: {2}", 
                result.IsValid, result.Errors.Count, result.Warnings.Count);

            return result;
        }

        public ValidationResult ValidatePath(string path, bool mustExist = true)
        {
            var result = new ValidationResult { IsValid = true };

            if (string.IsNullOrWhiteSpace(path))
            {
                result.AddError("Path cannot be null or empty");
                return result;
            }

            try
            {
                // Check for invalid characters
                var invalidChars = Path.GetInvalidPathChars();
                foreach (char c in invalidChars)
                {
                    if (path.Contains(c.ToString()))
                    {
                        result.AddError($"Path contains invalid character: {c}");
                        return result;
                    }
                }

                // Check if path is rooted
                if (!Path.IsPathRooted(path))
                {
                    result.AddWarning($"Path is not absolute: {path}");
                }

                // Check if path exists (if required)
                if (mustExist)
                {
                    var existsTask = _fileOperations.DirectoryExistsAsync(path);
                    existsTask.Wait();
                    
                    if (!existsTask.Result)
                    {
                        result.AddError($"Directory does not exist: {path}");
                    }
                }

                // Check write permissions (attempt to create a test file)
                try
                {
                    var testFilePath = Path.Combine(path, "test_write_access.tmp");
                    File.WriteAllText(testFilePath, "test");
                    File.Delete(testFilePath);
                }
                catch (UnauthorizedAccessException)
                {
                    result.AddError($"No write access to directory: {path}");
                }
                catch (DirectoryNotFoundException)
                {
                    if (mustExist)
                    {
                        result.AddError($"Directory not found: {path}");
                    }
                }
                catch (Exception ex)
                {
                    result.AddWarning($"Could not verify write access to {path}: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Path validation failed: {ex.Message}");
            }

            return result;
        }

        public ValidationResult ValidateEmailSettings()
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                // Validate FROM email address
                var fromEmail = Settings.Default.FromEmail;
                if (!string.IsNullOrWhiteSpace(fromEmail))
                {
                    if (!IsValidEmail(fromEmail))
                    {
                        result.AddError($"Invalid FROM email address: {fromEmail}");
                    }
                }
                else
                {
                    result.AddWarning("FROM email address is not configured");
                }

                // Validate phone number for SMS
                var phoneNumber = Settings.Default.PhoneNumber;
                if (!string.IsNullOrWhiteSpace(phoneNumber))
                {
                    if (!IsValidPhoneNumber(phoneNumber))
                    {
                        result.AddError($"Invalid phone number format: {phoneNumber}");
                    }
                }
                else
                {
                    result.AddWarning("Phone number is not configured for SMS notifications");
                }

                // Validate SMTP settings
                var smtpAddress = Settings.Default.SMTPAddress;
                var smtpPort = Settings.Default.SMTPPort;
                var smtpUser = Settings.Default.SMTPUser;
                var smtpPass = Settings.Default.SMTPPass;

                if (!string.IsNullOrWhiteSpace(smtpAddress))
                {
                    if (!IsValidHostname(smtpAddress))
                    {
                        result.AddError($"Invalid SMTP server address: {smtpAddress}");
                    }
                }
                else
                {
                    result.AddWarning("SMTP server address is not configured");
                }

                if (smtpPort <= 0 || smtpPort > 65535)
                {
                    result.AddError($"Invalid SMTP port: {smtpPort}. Must be between 1 and 65535");
                }

                if (string.IsNullOrWhiteSpace(smtpUser))
                {
                    result.AddWarning("SMTP username is not configured");
                }

                if (string.IsNullOrWhiteSpace(smtpPass))
                {
                    result.AddWarning("SMTP password is not configured");
                }

                // Validate carrier provider
                var provider = Settings.Default.CurrentProvider;
                if (string.IsNullOrWhiteSpace(provider))
                {
                    result.AddWarning("SMS carrier provider is not configured");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Email settings validation failed: {ex.Message}");
            }

            return result;
        }

        public ValidationResult ValidateHandbrakeSettings()
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                // Check Handbrake parameters
                var handbrakeParams = Settings.Default.HandBrakeParameters;
                if (!string.IsNullOrWhiteSpace(handbrakeParams))
                {
                    // Basic validation - check for dangerous characters
                    if (handbrakeParams.Contains("..") || handbrakeParams.Contains(";") || handbrakeParams.Contains("&"))
                    {
                        result.AddWarning("Handbrake parameters contain potentially unsafe characters");
                    }

                    // Check for required output format
                    if (!handbrakeParams.Contains(".mp4") && !handbrakeParams.Contains(".m4v"))
                    {
                        result.AddWarning("Handbrake parameters should specify output format (.mp4 or .m4v)");
                    }
                }
                else
                {
                    result.AddWarning("Handbrake parameters are not configured");
                }

                // Check if conversion is enabled but parameters are missing
                if (Settings.Default.ConvWithHandbrake && string.IsNullOrWhiteSpace(handbrakeParams))
                {
                    result.AddError("Handbrake conversion is enabled but parameters are not configured");
                }

                // Check Handbrake executable path
                var executablePath = AppDomain.CurrentDomain.BaseDirectory;
                var handbrakePath = Path.Combine(executablePath, "HandbrakeCLI", "HandbrakeCLI.exe");
                var handbrakeZip = Path.Combine(executablePath, "HandbrakeCLI.zip");

                var handbrakeExistsTask = _fileOperations.FileExistsAsync(handbrakePath);
                handbrakeExistsTask.Wait();

                if (!handbrakeExistsTask.Result)
                {
                    var zipExistsTask = _fileOperations.FileExistsAsync(handbrakeZip);
                    zipExistsTask.Wait();

                    if (!zipExistsTask.Result)
                    {
                        result.AddError("Handbrake CLI executable and zip file not found");
                    }
                    else
                    {
                        result.AddWarning("Handbrake CLI needs to be extracted from zip file");
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Handbrake settings validation failed: {ex.Message}");
            }

            return result;
        }

        public ValidationResult ValidateMakeMKVSettings()
        {
            var result = new ValidationResult { IsValid = true };

            try
            {
                // Check MakeMKV path
                var makemkvPath = Settings.Default.MakeMKVPath;
                if (!string.IsNullOrWhiteSpace(makemkvPath))
                {
                    var existsTask = _fileOperations.FileExistsAsync(makemkvPath);
                    existsTask.Wait();

                    if (!existsTask.Result)
                    {
                        result.AddError($"MakeMKV executable not found at: {makemkvPath}");
                    }
                }
                else
                {
                    result.AddError("MakeMKV path is not configured");
                }

                // Validate minimum title length
                var minTitleLength = Settings.Default.MinTitleLength;
                if (!string.IsNullOrWhiteSpace(minTitleLength))
                {
                    if (!int.TryParse(minTitleLength, out int minutes) || minutes < 0)
                    {
                        result.AddError($"Invalid minimum title length: {minTitleLength}. Must be a positive number");
                    }
                    else if (minutes < 5)
                    {
                        result.AddWarning($"Minimum title length is very short ({minutes} minutes). This may include unwanted content");
                    }
                    else if (minutes > 180)
                    {
                        result.AddWarning($"Minimum title length is very long ({minutes} minutes). This may exclude valid content");
                    }
                }
                else
                {
                    result.AddWarning("Minimum title length is not configured");
                }
            }
            catch (Exception ex)
            {
                result.AddError($"MakeMKV settings validation failed: {ex.Message}");
            }

            return result;
        }

        private ValidationResult ValidateAllPaths()
        {
            var result = new ValidationResult { IsValid = true };

            // Validate temp path
            var tempPath = Settings.Default.TempPath;
            if (!string.IsNullOrWhiteSpace(tempPath))
            {
                var tempValidation = ValidatePath(tempPath, false); // Don't require existence
                MergeValidationResults(result, tempValidation);
            }
            else
            {
                result.AddError("Temp path is not configured");
            }

            // Validate final path
            var finalPath = Settings.Default.FinalPath;
            if (!string.IsNullOrWhiteSpace(finalPath))
            {
                var finalValidation = ValidatePath(finalPath, false); // Don't require existence
                MergeValidationResults(result, finalValidation);
            }
            else
            {
                result.AddWarning("Final path is not configured - files will remain in temp location");
            }

            return result;
        }

        private void MergeValidationResults(ValidationResult target, ValidationResult source)
        {
            target.Errors.AddRange(source.Errors);
            target.Warnings.AddRange(source.Warnings);
            if (!source.IsValid)
            {
                target.IsValid = false;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Simple phone number validation - digits only, 10-15 characters
            return Regex.IsMatch(phoneNumber, @"^\d{10,15}$");
        }

        private bool IsValidHostname(string hostname)
        {
            try
            {
                return Uri.CheckHostName(hostname) != UriHostNameType.Unknown;
            }
            catch
            {
                return false;
            }
        }
    }
}
