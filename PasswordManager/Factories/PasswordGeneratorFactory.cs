using PasswordManager.Services;

namespace PasswordManager.Factories;

public static class PasswordGeneratorFactory
{
    public static PasswordGenerator CreateDefault()
    {
        return new PasswordGenerator();
    }
}
