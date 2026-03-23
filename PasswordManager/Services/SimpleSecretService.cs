using System.Security.Cryptography;
using System.Text;
using PasswordManager.Models;

namespace PasswordManager.Services;

public class SimpleSecretService : ISecretService
{
    public SecretBlob Protect(string plainText, string masterPassword)
    {
        var data = Encoding.UTF8.GetBytes(plainText);
        var key = BuildKey(masterPassword);

        var encrypted = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
            encrypted[i] = (byte)(data[i] ^ key[i % key.Length]);

        return new SecretBlob { Data = encrypted };
    }

    public string Reveal(SecretBlob blob, string masterPassword)
    {
        var key = BuildKey(masterPassword);
        var decrypted = new byte[blob.Data.Length];

        for (int i = 0; i < blob.Data.Length; i++)
            decrypted[i] = (byte)(blob.Data[i] ^ key[i % key.Length]);

        return Encoding.UTF8.GetString(decrypted);
    }

    private static byte[] BuildKey(string masterPassword)
    {
        return SHA256.HashData(Encoding.UTF8.GetBytes(masterPassword));
    }
}
