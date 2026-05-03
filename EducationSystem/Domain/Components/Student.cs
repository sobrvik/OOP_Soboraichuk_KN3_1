namespace Domain.Components;

using Domain.Exceptions;
using Domain.Visitors;

public sealed class Student : IUniversityComponent, IDisposable, IEquatable<Student>
{
    private string _name;
    private int _score;
    private bool _disposed;

    public Guid Id { get; }
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Ім'я студента не може бути порожнім.");
            _name = value.Trim();
        }
    }

    public int Score
    {
        get => _score;
        private set
        {
            if (value < 0 || value > 100)
                throw new DomainException("Рейтинг студента має бути від 0 до 100.");
            _score = value;
        }
    }

    public IReadOnlyList<IUniversityComponent> Children => Array.Empty<IUniversityComponent>();

    public Student(string name, int score)
    {
        Id = Guid.NewGuid();
        _name = string.Empty;
        Name = name;
        Score = score;
    }

    public Student(Student other)
    {
        Id = other.Id;
        _name = other.Name;
        _score = other.Score;
    }

    public void UpdateScore(int score)
    {
        Score = score;
    }

    public void Add(IUniversityComponent component)
    {
        throw new InvalidOperationException("До студента не можна додавати дочірні елементи.");
    }

    public void Remove(IUniversityComponent component)
    {
        throw new InvalidOperationException("У студента немає дочірніх елементів.");
    }

    public void Accept(IUniversityVisitor visitor)
    {
        visitor.Visit(this);
    }

    public bool Equals(Student? other)
    {
        return other is not null && Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Student);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Student? left, Student? right)
    {
        return EqualityComparer<Student>.Default.Equals(left, right);
    }

    public static bool operator !=(Student? left, Student? right)
    {
        return !(left == right);
    }

    public void Dispose()
    {
        _disposed = true;
    }

    public bool IsDisposed => _disposed;
}
