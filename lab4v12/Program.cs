using System;
using System.Collections.Generic;

namespace Lab4_v12
{
    // Інтерфейс для атаки
    public interface IAttack
    {
        int Attack();
    }

    // Абстрактний клас "Персонаж"
    public abstract class Character : IAttack
    {
        public string Name { get; set; }
        public int BaseDamage { get; set; }

        public Character(string name, int damage)
        {
            Name = name;
            BaseDamage = damage;
        }

        // Абстрактний метод, який реалізують спадкоємці
        public abstract int Attack();
    }

    // Warrior
    public class Warrior : Character
    {
        public Warrior(string name, int damage) : base(name, damage) {}

        public override int Attack()
        {
            Console.WriteLine($"{Name} вдаряє мечем на {BaseDamage} шкоди!");
            return BaseDamage;
        }
    }

    // Archer
    public class Archer : Character
    {
        public Archer(string name, int damage) : base(name, damage) {}

        public override int Attack()
        {
            int dmg = BaseDamage * 2;
            Console.WriteLine($"{Name} випускає стрілу на {dmg} шкоди!");
            return dmg;
        }
    }

    // Група
    public class Group
    {
        private List<Character> members = new List<Character>();

        public void AddMember(Character character)
        {
            members.Add(character);
        }

        public int GroupAttack()
        {
            int totalDamage = 0;
            foreach (var member in members)
            {
                totalDamage += member.Attack();
            }
            Console.WriteLine($"Сумарна шкода групи: {totalDamage}");
            return totalDamage;
        }
    }

    class Program
    {
        static void Main()
        {
            Warrior w = new Warrior("Іванов", 15);
            Archer a = new Archer("Сидоров", 10);

            Group g = new Group();
            g.AddMember(w);
            g.AddMember(a);

            g.GroupAttack();

            Console.ReadLine();
        }
    }
}