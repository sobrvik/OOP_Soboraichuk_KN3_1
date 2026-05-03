namespace Domain.Visitors;

using Domain.Components;

public sealed class RatingVisitor : IUniversityVisitor
{
    private readonly List<Student> _students = new();

    public IReadOnlyList<Student> Students => _students.AsReadOnly();

    public void Visit(Faculty faculty)
    {
    }

    public void Visit(Group group)
    {
    }

    public void Visit(Student student)
    {
        _students.Add(student);
    }

    public double AverageRating()
    {
        return _students.Count == 0 ? 0 : _students.Average(s => s.Score);
    }

    public Student? BestStudent()
    {
        return _students.OrderByDescending(s => s.Score).FirstOrDefault();
    }
}
