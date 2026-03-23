using System.Security.Cryptography;
using PasswordManager.Models;

namespace PasswordManager.Services;

public class PasswordGenerator
{
    private const string Lower = "abcdefghijklmnopqrstuvwxyz";
    private const string Upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Digits = "0123456789";
    private const string Symbols = "!@#$%^&*()-_=+[]{}";

    public string Generate(PasswordPolicy policy)
    {
        var characterSet = BuildCharacterSet(policy);

        if (string.IsNullOrWhiteSpace(characterSet))
            throw new InvalidOperationException("Не обрано жодної групи символів.");

        var result = new char[policy.Length];
        for (int i = 0; i < result.Length; i++)
            result[i] = characterSet[RandomNumberGenerator.GetInt32(characterSet.Length)];

        return new string(result);
    }

    private static string BuildCharacterSet(PasswordPolicy policy)
    {
        var chars = string.Empty;

        if (policy.UseLower) chars += Lower;
        if (policy.UseUpper) chars += Upper;
        if (policy.UseDigits) chars += Digits;
        if (policy.UseSymbols) chars += Symbols;

        return chars;
    }
}
