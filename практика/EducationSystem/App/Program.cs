using Domain.Components;
using Domain.Lessons;
using Domain.Repositories;
using Domain.Services;
using Domain.Visitors;

var faculty = new Faculty("Факультет інформаційних технологій");
var group = new Group("КН-31");

group += new Student("Соборайчук Віктор", 95);
group += new Student("Іваненко Іван", 82);
group += new Student("Петренко Петро", 74);

faculty.Add(group);

var ratingVisitor = new RatingVisitor();
faculty.Accept(ratingVisitor);

Console.WriteLine("=== Система управління навчальним закладом ===");
Console.WriteLine($"Факультет: {faculty.Name}");
Console.WriteLine($"Середній рейтинг: {ratingVisitor.AverageRating():F2}");
Console.WriteLine($"Кращий студент: {ratingVisitor.BestStudent()?.Name}");

var lesson = LessonFactory.Create("lecture", "Об'єктно-орієнтоване програмування", 90);
Console.WriteLine(lesson.Conduct());

var repository = new InMemoryRepository<Lesson>();
repository.Add(lesson);
repository.Add(LessonFactory.Create("lab", "Патерни проєктування", 80));

Console.WriteLine($"Занять у репозиторії: {repository.GetAll().Count}");

var storage = new JsonUniversityStorage();
var json = storage.SaveToJson(faculty);

Console.WriteLine();
Console.WriteLine("JSON-збереження стану:");
Console.WriteLine(json);
