namespace Domain.Visitors;

using Domain.Components;

public interface IUniversityVisitor
{
    void Visit(Faculty faculty);
    void Visit(Group group);
    void Visit(Student student);
}
