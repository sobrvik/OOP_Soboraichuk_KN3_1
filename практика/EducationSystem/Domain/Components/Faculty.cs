namespace Domain.Components;

using Domain.Exceptions;
using Domain.Visitors;

public sealed class Faculty : IUniversityComponent
{
    private readonly List<IUniversityComponent> _groups = new();

    public string Name { get; }
    public IReadOnlyList<IUniversityComponent> Children => _groups.AsReadOnly();

    public IUniversityComponent this[int index] => _groups[index];

    public Faculty(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Назва факультету не може бути порожньою.");

        Name = name.Trim();
    }

    public void Add(IUniversityComponent component)
    {
        if (component is not Group)
            throw new DomainException("У факультет можна додавати тільки групи.");

        _groups.Add(component);
    }

    public void Remove(IUniversityComponent component)
    {
        _groups.Remove(component);
    }

    public void Accept(IUniversityVisitor visitor)
    {
        visitor.Visit(this);

        foreach (var group in _groups)
            group.Accept(visitor);
    }
}
