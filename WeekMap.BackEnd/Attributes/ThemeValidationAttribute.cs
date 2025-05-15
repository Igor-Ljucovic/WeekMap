using System.ComponentModel.DataAnnotations;

namespace WeekMap.Attributes
{
    public class ThemeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var theme = value as string;
            if (theme == "light" || theme == "dark") return ValidationResult.Success;
            return new ValidationResult(ErrorMessage);
        }
    }
}
