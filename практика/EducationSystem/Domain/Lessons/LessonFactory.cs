namespace Domain.Lessons;

public static class LessonFactory
{
    public static Lesson Create(string type, string title, int durationMinutes)
    {
        return type.Trim().ToLowerInvariant() switch
        {
            "lecture" or "лекція" => new Lecture(title, durationMinutes),
            "practice" or "практика" => new PracticeLesson(title, durationMinutes),
            "lab" or "лабораторна" => new LabLesson(title, durationMinutes),
            _ => throw new ArgumentException($"Невідомий тип заняття: {type}")
        };
    }
}
