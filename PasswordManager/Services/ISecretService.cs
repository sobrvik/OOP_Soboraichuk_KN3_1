using PasswordManager.Models;

namespace PasswordManager.Services;

public interface ISecretService
{
    SecretBlob Protect(string plainText, string masterPassword);
    string Reveal(SecretBlob blob, string masterPassword);
}
