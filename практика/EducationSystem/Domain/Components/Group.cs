namespace Domain.Components;

using Domain.Exceptions;
using Domain.Visitors;

public sealed class Group : IUniversityComponent
{
    private readonly List<IUniversityComponent> _students = new();

    public string Name { get; }
    public IReadOnlyList<IUniversityComponent> Children => _students.AsReadOnly();

    public IUniversityComponent this[int index] => _students[index];

    public Group(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Назва групи не може бути порожньою.");

        Name = name.Trim();
    }

    public void Add(IUniversityComponent component)
    {
        if (component is not Student)
            throw new DomainException("У групу можна додавати тільки студентів.");

        _students.Add(component);
    }

    public void Remove(IUniversityComponent component)
    {
        _students.Remove(component);
    }

    public void Accept(IUniversityVisitor visitor)
    {
        visitor.Visit(this);

        foreach (var student in _students)
            student.Accept(visitor);
    }

    public static Group operator +(Group group, Student student)
    {
        group.Add(student);
        return group;
    }
}
