using System.ComponentModel.DataAnnotations;

namespace WeekMap.Attributes
{
    public class ValidWeekMapTimeRangeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;

            var dayStartTimeProp = context.ObjectType.GetProperty("DayStartTime");
            var dayEndTimeProp = context.ObjectType.GetProperty("DayEndTime");

            if (dayStartTimeProp == null || dayEndTimeProp == null)
                return ValidationResult.Success;

            var start = (TimeSpan)dayStartTimeProp.GetValue(instance);
            var end = (TimeSpan)dayEndTimeProp.GetValue(instance);

            var duration = end - start;

            if (start < TimeSpan.Zero || end < TimeSpan.Zero)
                return new ValidationResult("Start and end times must be non-negative.");

            if (duration < TimeSpan.FromHours(1) || duration > TimeSpan.FromHours(24))
                return new ValidationResult("The time range must be between 1 and 24 hours.");

            if (start.Hours >= 24 || end.Hours >= 24)
                return new ValidationResult("The time range must be an integer number of hours (e.g., 1, 2, ..., 23, 0).");

            if (duration.Minutes != 0 || duration.Seconds != 0 || duration.Milliseconds != 0)
                return new ValidationResult("The time range must be an integer number of hours (e.g., 1, 2, ..., 23, 0).");

            return ValidationResult.Success;
        }
    }
}