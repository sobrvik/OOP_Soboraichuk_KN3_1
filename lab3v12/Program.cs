using System;
using System.Collections.Generic;

// Базовий клас
class Course
{
    public string Name { get; set; }     // назва курсу
    public int Credits { get; set; }     // кількість кредитів

    public Course(string name, int credits)
    {
        Name = name;
        Credits = credits;
    }

    // Віртуальний метод для виводу інформації
    public virtual void ShowInfo()
    {
        Console.WriteLine($"Курс: {Name}, Кредити: {Credits}");
    }
}

// Похідний клас для математики
class MathCourse : Course
{
    public string Level { get; set; } // рівень (наприклад: базовий, просунутий)

    public MathCourse(string name, int credits, string level)
        : base(name, credits) // виклик конструктора базового класу
    {
        Level = level;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Математичний курс: {Name}, Кредити: {Credits}, Рівень: {Level}");
    }
}

// Похідний клас для історії
class HistoryCourse : Course
{
    public string Period { get; set; } // історичний період

    public HistoryCourse(string name, int credits, string period)
        : base(name, credits)
    {
        Period = period;
    }

    public override void ShowInfo()
    {
        Console.WriteLine($"Історичний курс: {Name}, Кредити: {Credits}, Період: {Period}");
    }
}

class Program
{
    static void Main()
    {
        // Колекція курсів (демонстрація поліморфізму)
        List<Course> courses = new List<Course>
        {
            new MathCourse("Лінійна алгебра", 5, "просунутий"),
            new HistoryCourse("Історія України", 4, "XX століття"),
            new MathCourse("Математичний аналіз", 6, "базовий")
        };

        // Виведення всіх курсів
        Console.WriteLine("Список курсів:");
        foreach (var course in courses)
        {
            course.ShowInfo();
        }

        // Обчислення загальної кількості кредитів
        int totalCredits = 0;
        foreach (var course in courses)
        {
            totalCredits += course.Credits;
        }

        double averageCredits = (double)totalCredits / courses.Count;

        Console.WriteLine($"\nЗагальна кількість кредитів: {totalCredits}");
        Console.WriteLine($"Середнє навантаження: {averageCredits:F2}");
    }
}