using System.Collections.Generic;

namespace AutoRip2MKV
{
    public interface IConfigurationValidator
    {
        ValidationResult ValidateConfiguration();
        ValidationResult ValidatePath(string path, bool mustExist = true);
        ValidationResult ValidateEmailSettings();
        ValidationResult ValidateHandbrakeSettings();
        ValidationResult ValidateMakeMKVSettings();
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
            IsValid = false;
        }

        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }
    }
}
