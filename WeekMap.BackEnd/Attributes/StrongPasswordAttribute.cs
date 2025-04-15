using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class StrongPasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var password = value as string;

        // can't contain non-ASCII characters
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, @"^[\x20-\x7E]+$"))
            return false;

        return password.Length >= 8
            && password.Length <= 64
            && Regex.IsMatch(password, @"[A-Z]")    // at least 1 uppercase character
            && Regex.IsMatch(password, @"[0-9]");   // at least one digit
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be at least 8 characters long, at most 64 characters long, contain at least one uppercase letter and one number," +
            $" and only contain letters, numbers or symbols (no emojis or non-english characters)";
    }
}
