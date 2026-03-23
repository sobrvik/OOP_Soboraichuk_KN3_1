namespace PasswordManager.Configuration;

public sealed class AppConfig
{
    private static readonly Lazy<AppConfig> _instance = new(() => new AppConfig());

    public static AppConfig Instance => _instance.Value;

    public string MasterPassword { get; } = "1234";

    private AppConfig()
    {
    }
}
