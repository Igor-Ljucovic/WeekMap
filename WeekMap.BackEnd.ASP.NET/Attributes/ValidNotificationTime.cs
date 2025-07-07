using System.ComponentModel.DataAnnotations;

namespace WeekMap.Attributes
{
    public class ValidNotificationTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var instance = context.ObjectInstance;

            var notificationTime = context.ObjectType.GetProperty("NotificationTime");

            var time = (TimeSpan)notificationTime.GetValue(instance);

            // time.Days > 1 because if hours > 24 it automatically looks for days 
            if (time < TimeSpan.Zero || time.Days > 0)
                return new ValidationResult("Minimum valid notification time is 00:00:00, maximum is 23:59:59.");

            return ValidationResult.Success;
        }
    }
}