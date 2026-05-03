using Domain.Components;
using Domain.Exceptions;
using Domain.Lessons;
using Domain.Services;
using Domain.Visitors;
using Xunit;

namespace Tests;

public class UniversityTests
{
    [Fact]
    public void RatingVisitor_ShouldCalculateAverageRating()
    {
        var group = new Group("КН-31");
        group.Add(new Student("A", 100));
        group.Add(new Student("B", 50));

        var visitor = new RatingVisitor();
        group.Accept(visitor);

        Assert.Equal(75, visitor.AverageRating());
    }

    [Fact]
    public void Student_ShouldRejectInvalidScore()
    {
        Assert.Throws<DomainException>(() => new Student("Test", 120));
    }

    [Fact]
    public void LessonFactory_ShouldCreateLecture()
    {
        var lesson = LessonFactory.Create("lecture", "OOP", 90);

        Assert.IsType<Lecture>(lesson);
        Assert.Contains("Лекція", lesson.Conduct());
    }

    [Fact]
    public void Composite_ShouldRejectGroupInsideGroup()
    {
        var group = new Group("КН-31");

        Assert.Throws<DomainException>(() => group.Add(new Group("КН-32")));
    }

    [Fact]
    public void JsonStorage_ShouldSaveAndLoadFaculty()
    {
        var faculty = new Faculty("IT");
        var group = new Group("КН-31");
        group.Add(new Student("Viktor", 90));
        faculty.Add(group);

        var storage = new JsonUniversityStorage();

        var json = storage.SaveToJson(faculty);
        var restored = storage.LoadFromJson(json);

        Assert.Equal("IT", restored.Name);
        Assert.Single(restored.Children);
    }
}
