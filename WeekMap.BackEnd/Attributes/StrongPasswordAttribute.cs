using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class StrongPasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var password = value as string;

        // can only contain alphanumerics or symbols
        if (string.IsNullOrEmpty(password) || !Regex.IsMatch(password, @"^[\x21-\x7E]+$"))
            return false;

        return password.Length >= 8
            && password.Length <= 64
            && Regex.IsMatch(password, @"[A-Z]")    // at least 1 uppercase character
            && Regex.IsMatch(password, @"[0-9]");   // at least one digit
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must contain at least one uppercase letter and one number," +
            $" and only contain letters, numbers or symbols (no emojis or non-english characters). It can't have whitespaces";
    }
}
