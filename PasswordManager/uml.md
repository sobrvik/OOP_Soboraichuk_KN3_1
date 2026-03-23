classDiagram
    class Program {
        +Main()
        +AddEntry()
        +ListEntries()
        +ShowPassword()
        +UpdateEntry()
        +DeleteEntry()
        +GeneratePassword()
    }

    class Vault {
        +List~AccountEntry~ Entries
    }

    class AccountEntry {
        +Guid Id
        +string Title
        +string Username
        +string Category
        +List~string~ Tags
        +SecretBlob Password
    }

    class SecretBlob {
        +byte[] Data
    }

    class PasswordPolicy {
        +int Length
        +bool UseLower
        +bool UseUpper
        +bool UseDigits
        +bool UseSymbols
    }

    class ISecretService {
        <<interface>>
        +Protect(string plainText, string masterPassword) SecretBlob
        +Reveal(SecretBlob blob, string masterPassword) string
    }

    class SimpleSecretService {
        +Protect(string plainText, string masterPassword) SecretBlob
        +Reveal(SecretBlob blob, string masterPassword) string
        -BuildKey(string masterPassword) byte[]
    }

    class PasswordGenerator {
        +Generate(PasswordPolicy policy) string
        -BuildCharacterSet(PasswordPolicy policy) string
    }

    class VaultService {
        -ISecretService secretService
        +AddEntry(Vault vault, string title, string username, string password, string category, List~string~ tags, string masterPassword)
        +GetById(Vault vault, Guid id) AccountEntry
        +RevealPassword(AccountEntry entry, string masterPassword) string
        +UpdateEntry(Vault vault, Guid id, string title, string username, string newPassword, string category, List~string~ tags, string masterPassword)
        +DeleteEntry(Vault vault, Guid id) bool
    }

    class PasswordGeneratorFactory {
        <<static>>
        +CreateDefault() PasswordGenerator
    }

    class AppConfig {
        <<singleton>>
        +Instance AppConfig
        +MasterPassword string
    }

    Program --> Vault
    Program --> VaultService
    Program --> PasswordGenerator
    Program --> AppConfig

    Vault --> AccountEntry
    AccountEntry --> SecretBlob

    VaultService --> Vault
    VaultService --> AccountEntry
    VaultService --> ISecretService

    SimpleSecretService ..|> ISecretService

    PasswordGenerator --> PasswordPolicy
    PasswordGeneratorFactory --> PasswordGenerator