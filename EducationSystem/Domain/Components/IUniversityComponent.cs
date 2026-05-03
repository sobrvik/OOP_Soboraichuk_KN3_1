namespace Domain.Components;

using Domain.Visitors;

public interface IUniversityComponent
{
    string Name { get; }
    void Add(IUniversityComponent component);
    void Remove(IUniversityComponent component);
    void Accept(IUniversityVisitor visitor);
    IReadOnlyList<IUniversityComponent> Children { get; }
}
