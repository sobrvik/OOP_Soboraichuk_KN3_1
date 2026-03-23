using PasswordManager.Models;
using PasswordManager.Services;
using PasswordManager.Factories;
using PasswordManager.Configuration;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var vault = new Vault();
var secretService = new SimpleSecretService();
var vaultService = new VaultService(secretService);
var generator = PasswordGeneratorFactory.CreateDefault();
var appConfig = AppConfig.Instance;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== МЕНЕДЖЕР ПАРОЛІВ ===");
    Console.WriteLine("1. Додати запис");
    Console.WriteLine("2. Показати всі записи");
    Console.WriteLine("3. Переглянути пароль");
    Console.WriteLine("4. Оновити запис");
    Console.WriteLine("5. Видалити запис");
    Console.WriteLine("6. Згенерувати пароль");
    Console.WriteLine("0. Вихід");
    Console.Write("Оберіть дію: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            AddEntry(vaultService, vault, appConfig.MasterPassword);
            break;
        case "2":
            ListEntries(vault);
            break;
        case "3":
            ShowPassword(vaultService, vault, appConfig.MasterPassword);
            break;
        case "4":
            UpdateEntry(vaultService, vault, appConfig.MasterPassword);
            break;
        case "5":
            DeleteEntry(vaultService, vault);
            break;
        case "6":
            GeneratePassword(generator);
            break;
        case "0":
            return;
        default:
            Console.WriteLine("Невірний вибір.");
            break;
    }
}

static void AddEntry(VaultService vaultService, Vault vault, string masterPassword)
{
    Console.Write("Назва сервісу: ");
    var title = Console.ReadLine() ?? string.Empty;

    Console.Write("Логін: ");
    var username = Console.ReadLine() ?? string.Empty;

    Console.Write("Категорія: ");
    var category = Console.ReadLine() ?? string.Empty;

    Console.Write("Теги (через кому): ");
    var tagsInput = Console.ReadLine() ?? string.Empty;
    var tags = tagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

    Console.Write("Пароль: ");
    var password = Console.ReadLine() ?? string.Empty;

    vaultService.AddEntry(vault, title, username, password, category, tags, masterPassword);
    Console.WriteLine("Запис успішно додано.");
}

static void ListEntries(Vault vault)
{
    if (vault.Entries.Count == 0)
    {
        Console.WriteLine("Сховище порожнє.");
        return;
    }

    Console.WriteLine("Список записів:");
    foreach (var entry in vault.Entries)
    {
        Console.WriteLine($"ID: {entry.Id}");
        Console.WriteLine($"Назва: {entry.Title}");
        Console.WriteLine($"Логін: {entry.Username}");
        Console.WriteLine($"Категорія: {entry.Category}");
        Console.WriteLine($"Теги: {string.Join(", ", entry.Tags)}");
        Console.WriteLine(new string('-', 30));
    }
}

static void ShowPassword(VaultService vaultService, Vault vault, string masterPassword)
{
    Console.Write("Введіть ID запису: ");
    if (!Guid.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Некоректний ID.");
        return;
    }

    var entry = vaultService.GetById(vault, id);
    if (entry is null)
    {
        Console.WriteLine("Запис не знайдено.");
        return;
    }

    var password = vaultService.RevealPassword(entry, masterPassword);
    Console.WriteLine($"Пароль: {password}");
}

static void UpdateEntry(VaultService vaultService, Vault vault, string masterPassword)
{
    Console.Write("Введіть ID запису: ");
    if (!Guid.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Некоректний ID.");
        return;
    }

    var entry = vaultService.GetById(vault, id);
    if (entry is null)
    {
        Console.WriteLine("Запис не знайдено.");
        return;
    }

    Console.Write($"Нова назва ({entry.Title}): ");
    var title = Console.ReadLine();
    Console.Write($"Новий логін ({entry.Username}): ");
    var username = Console.ReadLine();
    Console.Write($"Нова категорія ({entry.Category}): ");
    var category = Console.ReadLine();
    Console.Write($"Нові теги ({string.Join(", ", entry.Tags)}): ");
    var tagsInput = Console.ReadLine();
    Console.Write("Новий пароль (можна залишити порожнім): ");
    var password = Console.ReadLine();

    var tags = string.IsNullOrWhiteSpace(tagsInput)
        ? entry.Tags
        : tagsInput.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

    vaultService.UpdateEntry(
        vault,
        id,
        string.IsNullOrWhiteSpace(title) ? entry.Title : title,
        string.IsNullOrWhiteSpace(username) ? entry.Username : username,
        string.IsNullOrWhiteSpace(password) ? null : password,
        string.IsNullOrWhiteSpace(category) ? entry.Category : category,
        tags,
        masterPassword);

    Console.WriteLine("Запис оновлено.");
}

static void DeleteEntry(VaultService vaultService, Vault vault)
{
    Console.Write("Введіть ID запису: ");
    if (!Guid.TryParse(Console.ReadLine(), out var id))
    {
        Console.WriteLine("Некоректний ID.");
        return;
    }

    if (vaultService.DeleteEntry(vault, id))
        Console.WriteLine("Запис видалено.");
    else
        Console.WriteLine("Запис не знайдено.");
}

static void GeneratePassword(PasswordGenerator generator)
{
    var policy = new PasswordPolicy();

    Console.Write("Довжина пароля (за замовчуванням 12): ");
    var lengthInput = Console.ReadLine();
    if (int.TryParse(lengthInput, out var length) && length >= 4)
        policy.Length = length;

    Console.Write("Використовувати великі літери? (т/н): ");
    policy.UseUpper = (Console.ReadLine() ?? "т").Trim().ToLower() != "н";

    Console.Write("Використовувати малі літери? (т/н): ");
    policy.UseLower = (Console.ReadLine() ?? "т").Trim().ToLower() != "н";

    Console.Write("Використовувати цифри? (т/н): ");
    policy.UseDigits = (Console.ReadLine() ?? "т").Trim().ToLower() != "н";

    Console.Write("Використовувати спеціальні символи? (т/н): ");
    policy.UseSymbols = (Console.ReadLine() ?? "н").Trim().ToLower() == "т";

    var password = generator.Generate(policy);
    Console.WriteLine($"Згенерований пароль: {password}");
}
