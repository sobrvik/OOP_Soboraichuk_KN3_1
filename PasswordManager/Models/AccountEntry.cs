namespace PasswordManager.Models;

public class AccountEntry
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
    public SecretBlob Password { get; set; } = new();
}
