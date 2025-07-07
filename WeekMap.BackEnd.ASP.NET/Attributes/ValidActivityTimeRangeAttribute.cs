using System.ComponentModel.DataAnnotations;

namespace WeekMap.Attributes
{
    public class ValidActivityTimeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext context)
        {
            var instance = context.ObjectInstance;

            if (instance == null)
                return new ValidationResult("Instance is null.");

            // reflection
            var startProp = context.ObjectType.GetProperty("StartTime");
            var endProp = context.ObjectType.GetProperty("EndTime");

            if (startProp == null || endProp == null)
                return new ValidationResult("StartTime and EndTime properties are required.");

            var start = (TimeSpan?)startProp.GetValue(instance);
            var end = (TimeSpan?)endProp.GetValue(instance);

            if (start == null || end == null)
                return new ValidationResult("StartTime and EndTime must be set.");

            if (start >= end)
                return new ValidationResult("EndTime must be greater than StartTime.");

            if (start < TimeSpan.Zero || end < TimeSpan.Zero)
                return new ValidationResult("StartTime and EndTime must be positive.");

            if (start.Value.Days > 0 || end.Value.Days > 0)
                return new ValidationResult("StartTime and EndTime must be less than 24 hours.");

            return ValidationResult.Success!;
        }
    }
}