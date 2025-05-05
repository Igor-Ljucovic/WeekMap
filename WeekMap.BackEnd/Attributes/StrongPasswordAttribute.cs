using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

public class StrongPasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        var password = value as string;

        if (string.IsNullOrWhiteSpace(password))
            return false;

        // ASCII-only
        if (Encoding.UTF8.GetByteCount(password) != password.Length)
            return false;

        if (password.Any(char.IsWhiteSpace))
            return false;

        // At least 1 uppercase letter
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return false;

        // At least 1 digit 
        if (!Regex.IsMatch(password, @"\d"))
            return false;

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be 8–64 characters long, contain at least one uppercase letter and one number, " +
               $"and only contain ASCII characters without whitespaces (no emojis or non-English letters).";
    }
}
