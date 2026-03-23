using PasswordManager.Models;

namespace PasswordManager.Services;

public class VaultService
{
    private readonly ISecretService _secretService;

    public VaultService(ISecretService secretService)
    {
        _secretService = secretService;
    }

    public void AddEntry(
        Vault vault,
        string title,
        string username,
        string password,
        string category,
        List<string> tags,
        string masterPassword)
    {
        var entry = new AccountEntry
        {
            Id = Guid.NewGuid(),
            Title = title,
            Username = username,
            Category = category,
            Tags = tags,
            Password = _secretService.Protect(password, masterPassword)
        };

        vault.Entries.Add(entry);
    }

    public AccountEntry? GetById(Vault vault, Guid id)
    {
        return vault.Entries.FirstOrDefault(x => x.Id == id);
    }

    public string RevealPassword(AccountEntry entry, string masterPassword)
    {
        return _secretService.Reveal(entry.Password, masterPassword);
    }

    public void UpdateEntry(
        Vault vault,
        Guid id,
        string title,
        string username,
        string? newPassword,
        string category,
        List<string> tags,
        string masterPassword)
    {
        var entry = GetById(vault, id);
        if (entry is null)
            return;

        entry.Title = title;
        entry.Username = username;
        entry.Category = category;
        entry.Tags = tags;

        if (!string.IsNullOrWhiteSpace(newPassword))
            entry.Password = _secretService.Protect(newPassword, masterPassword);
    }

    public bool DeleteEntry(Vault vault, Guid id)
    {
        var entry = GetById(vault, id);
        if (entry is null)
            return false;

        vault.Entries.Remove(entry);
        return true;
    }
}
