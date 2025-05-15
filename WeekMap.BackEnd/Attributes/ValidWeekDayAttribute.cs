using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace WeekMap.Attributes
{
    public class ValidWeekDayAttribute : ValidationAttribute
    {
        // Now uses case-sensitive comparison
        private static readonly HashSet<string> ValidDays = new HashSet<string>(
            CultureInfo.InvariantCulture.DateTimeFormat.DayNames
        );

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string day && ValidDays.Contains(day))
                return ValidationResult.Success;

            return new ValidationResult("Week start day must be one of: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday (case-sensitive).");
        }
    }
}