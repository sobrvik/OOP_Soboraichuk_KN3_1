using System;
using System.Collections.Generic;

namespace lab22v10
{

    public class Employee_Broken
    {
        public string Name { get; }
        public decimal HourRate { get; }
        public int HoursWorked { get; }

        public Employee_Broken(string name, decimal hourRate, int hoursWorked)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
            if (hourRate < 0) throw new ArgumentOutOfRangeException(nameof(hourRate));
            if (hoursWorked < 0) throw new ArgumentOutOfRangeException(nameof(hoursWorked));

            Name = name;
            HourRate = hourRate;
            HoursWorked = hoursWorked;
        }

        // Контракт: повернути суму зарплати до виплати
        public virtual decimal CalculateSalary() => HourRate * HoursWorked;
    }

    // Варіант порушення: виняток ламає клієнтський метод, який очікує число
    public class Volunteer_Broken_Throws : Employee_Broken
    {
        public Volunteer_Broken_Throws(string name) : base(name, 0m, 0) { }

        public override decimal CalculateSalary()
            => throw new InvalidOperationException("Volunteer does not have salary.");
    }

    // Варіант порушення: 0 спотворює бізнес-логіку (волонтер не є paid employee)
    public class Volunteer_Broken_ReturnsZero : Employee_Broken
    {
        public Volunteer_Broken_ReturnsZero(string name) : base(name, 0m, 0) { }

        public override decimal CalculateSalary() => 0m;
    }

    public static class PayrollService_Broken
    {
        // Клієнтський метод: очікує, що будь-який Employee_Broken має коректну зарплату
        public static void SalaryReport(IEnumerable<Employee_Broken> employees)
        {
            Console.WriteLine("=== BROKEN Salary Report (Employee_Broken) ===");

            decimal total = 0m;
            foreach (var e in employees)
            {
                var salary = e.CalculateSalary(); // тут і проявляється LSP-порушення
                Console.WriteLine($"{e.Name,-22} salary = {salary,8}");
                total += salary;
            }

            Console.WriteLine($"TOTAL payroll = {total}");
            Console.WriteLine();
        }
    }

    public interface IPerson
    {
        string Name { get; }
    }

    public interface IPaidWorker : IPerson
    {
        decimal CalculateSalary();
    }

    public sealed class Employee : IPaidWorker
    {
        public string Name { get; }
        public decimal HourRate { get; }
        public int HoursWorked { get; }

        public Employee(string name, decimal hourRate, int hoursWorked)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
            if (hourRate < 0) throw new ArgumentOutOfRangeException(nameof(hourRate));
            if (hoursWorked < 0) throw new ArgumentOutOfRangeException(nameof(hoursWorked));

            Name = name;
            HourRate = hourRate;
            HoursWorked = hoursWorked;
        }

        public decimal CalculateSalary() => HourRate * HoursWorked;
    }

    public sealed class Volunteer : IPerson
    {
        public string Name { get; }

        public Volunteer(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
            Name = name;
        }
    }

    public static class PayrollService
    {
        // Працює тільки з тими, хто має контракт "отримує зарплату"
        public static void SalaryReport(IEnumerable<IPaidWorker> paidWorkers)
        {
            Console.WriteLine("=== FIXED Salary Report (IPaidWorker) ===");

            decimal total = 0m;
            foreach (var w in paidWorkers)
            {
                var salary = w.CalculateSalary();
                Console.WriteLine($"{w.Name,-22} salary = {salary,8}");
                total += salary;
            }

            Console.WriteLine($"TOTAL payroll = {total}");
            Console.WriteLine();
        }

        // Окремий метод для списку людей (і волонтерів теж)
        public static void PeopleList(IEnumerable<IPerson> people)
        {
            Console.WriteLine("=== People list (IPerson) ===");
            foreach (var p in people)
                Console.WriteLine(p.Name);
            Console.WriteLine();
        }
    }

    internal class Program
    {
        static void Main()
        {
            // --- Демонстрація LSP-порушення (Throws) ---
            var brokenThrows = new List<Employee_Broken>
            {
                new Employee_Broken("Ivan (employee)", 10m, 160),
                new Volunteer_Broken_Throws("Oksana (volunteer)")
            };

            try
            {
                PayrollService_Broken.SalaryReport(brokenThrows);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BROKEN version crashed:");
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
                Console.WriteLine();
            }

            // --- Демонстрація LSP-порушення (Returns 0) ---
            var brokenZero = new List<Employee_Broken>
            {
                new Employee_Broken("Petro (employee)", 12m, 160),
                new Volunteer_Broken_ReturnsZero("Andrii (volunteer)")
            };

            PayrollService_Broken.SalaryReport(brokenZero);
            Console.WriteLine("Note: volunteer looks like 'salary=0' -> business-logically incorrect.\n");

            // --- LSP-сумісне рішення ---
            var paidWorkers = new List<IPaidWorker>
            {
                new Employee("Ivan (employee)", 10m, 160),
                new Employee("Petro (employee)", 12m, 160)
            };

            var people = new List<IPerson>
            {
                new Employee("Ivan (employee)", 10m, 160),
                new Employee("Petro (employee)", 12m, 160),
                new Volunteer("Oksana (volunteer)"),
                new Volunteer("Andrii (volunteer)")
            };

            PayrollService.SalaryReport(paidWorkers);
            PayrollService.PeopleList(people);

            Console.WriteLine("Done.");
        }
    }
}