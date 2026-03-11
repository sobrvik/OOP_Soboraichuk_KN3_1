# Лабораторна робота №23

## Тема
**ISP & DIP: рефакторинг і Dependency Injection через конструктор**

## Мета роботи
Застосувати принципи **ISP (Interface Segregation Principle)** та **DIP (Dependency Inversion Principle)** для рефакторингу коду та реалізувати **Dependency Injection (DI)** через конструктор.

---

## Варіант завдання

**10. Game Character Action (HeroAction)**

Клас `HeroAction` виконує такі дії:

- атака
- лікування
- діалог з NPC

Залежності:

- `WeaponSystem`
- `MedicalKit`
- `DialogueManager`

---

## Аналіз проблем початкової реалізації

### Порушення ISP

Інтерфейс дій героя був занадто великий і містив методи:

- `Attack()`
- `Heal()`
- `Talk()`

Персонажі, які використовують тільки одну дію, змушені залежати від усіх методів.

### Порушення DIP

Клас `HeroAction` створював залежності самостійно:

```csharp
private WeaponSystem weapon = new WeaponSystem();
private MedicalKit kit = new MedicalKit();
private DialogueManager dialogue = new DialogueManager();
```

Клас високого рівня залежить від конкретних реалізацій.

---

## Виконаний рефакторинг

### Реалізація ISP

Інтерфейс було розділено на кілька вузьких:

- `IAttackAction`
- `IHealAction`
- `ITalkAction`

Тепер класи реалізують тільки необхідні методи.

### Реалізація DIP

Залежності замінено на абстракції:

- `IWeaponSystem`
- `IMedicalKit`
- `IDialogueManager`

### Dependency Injection

Залежності передаються через конструктор:

```csharp
public HeroAction(
    IWeaponSystem weaponSystem,
    IMedicalKit medicalKit,
    IDialogueManager dialogueManager)
{
    _weaponSystem = weaponSystem;
    _medicalKit = medicalKit;
    _dialogueManager = dialogueManager;
}
```

Конфігурація залежностей виконується у `Main()`.

---

## Демонстрація роботи

У методі `Main()` створюються об'єкти залежностей та передаються у `HeroAction`:

```csharp
IWeaponSystem weaponSystem = new WeaponSystem();
IMedicalKit medicalKit = new MedicalKit();
IDialogueManager dialogueManager = new DialogueManager();

HeroAction hero = new HeroAction(weaponSystem, medicalKit, dialogueManager);
```

---

## Результат виконання програми

```
=== Демонстрація роботи HeroAction ===
Герой завдає удару мечем.
Герой лікує себе аптечкою.
Герой розмовляє з NPC.

=== Демонстрація ISP: Warrior тільки атакує ===
Герой завдає удару мечем.
```

---

## Висновок

У лабораторній роботі застосовано принципи **ISP** та **DIP**.

Було виконано:

- розділення великого інтерфейсу на спеціалізовані
- використання абстракцій замість конкретних класів
- впровадження залежностей через конструктор (Dependency Injection)

У результаті код став більш гнучким, менш зв’язаним та простішим для тестування і розширення.