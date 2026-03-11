using System;

namespace lab23
{
    // ISP: вузькі інтерфейси дій героя
    public interface IAttackAction
    {
        void Attack();
    }

    public interface IHealAction
    {
        void Heal();
    }

    public interface ITalkAction
    {
        void Talk();
    }

    // DIP: абстракції для залежностей
    public interface IWeaponSystem
    {
        void Strike();
    }

    public interface IMedicalKit
    {
        void UseKit();
    }

    public interface IDialogueManager
    {
        void StartDialogue();
    }

    // Конкретні реалізації
    public class WeaponSystem : IWeaponSystem
    {
        public void Strike()
        {
            Console.WriteLine("Герой завдає удару мечем.");
        }
    }

    public class MedicalKit : IMedicalKit
    {
        public void UseKit()
        {
            Console.WriteLine("Герой лікує себе аптечкою.");
        }
    }

    public class DialogueManager : IDialogueManager
    {
        public void StartDialogue()
        {
            Console.WriteLine("Герой розмовляє з NPC.");
        }
    }

    // Головний клас, який використовує залежності через конструктор
    public class HeroAction : IAttackAction, IHealAction, ITalkAction
    {
        private readonly IWeaponSystem _weaponSystem;
        private readonly IMedicalKit _medicalKit;
        private readonly IDialogueManager _dialogueManager;

        public HeroAction(IWeaponSystem weaponSystem, IMedicalKit medicalKit, IDialogueManager dialogueManager)
        {
            _weaponSystem = weaponSystem;
            _medicalKit = medicalKit;
            _dialogueManager = dialogueManager;
        }

        public void Attack()
        {
            _weaponSystem.Strike();
        }

        public void Heal()
        {
            _medicalKit.UseKit();
        }

        public void Talk()
        {
            _dialogueManager.StartDialogue();
        }
    }

    // Додатковий клас для демонстрації ISP:
    // цей персонаж тільки атакує і не залежить від лікування та діалогів
    public class Warrior : IAttackAction
    {
        private readonly IWeaponSystem _weaponSystem;

        public Warrior(IWeaponSystem weaponSystem)
        {
            _weaponSystem = weaponSystem;
        }

        public void Attack()
        {
            _weaponSystem.Strike();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Налаштування залежностей у Main (DI через конструктор)
            IWeaponSystem weaponSystem = new WeaponSystem();
            IMedicalKit medicalKit = new MedicalKit();
            IDialogueManager dialogueManager = new DialogueManager();

            Console.WriteLine("=== Демонстрація роботи HeroAction ===");
            HeroAction hero = new HeroAction(weaponSystem, medicalKit, dialogueManager);

            hero.Attack();
            hero.Heal();
            hero.Talk();

            Console.WriteLine();

            Console.WriteLine("=== Демонстрація ISP: Warrior тільки атакує ===");
            Warrior warrior = new Warrior(weaponSystem);
            warrior.Attack();
        }
    }
}
