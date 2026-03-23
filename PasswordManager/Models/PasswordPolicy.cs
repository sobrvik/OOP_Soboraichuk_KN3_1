namespace PasswordManager.Models;

public class PasswordPolicy
{
    public int Length { get; set; } = 12;
    public bool UseLower { get; set; } = true;
    public bool UseUpper { get; set; } = true;
    public bool UseDigits { get; set; } = true;
    public bool UseSymbols { get; set; } = false;
}
