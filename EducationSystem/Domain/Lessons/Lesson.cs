namespace Domain.Lessons;

public abstract class Lesson
{
    protected Lesson(string title, int durationMinutes)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва заняття не може бути порожньою.", nameof(title));

        if (durationMinutes <= 0)
            throw new ArgumentException("Тривалість заняття має бути більшою за 0.", nameof(durationMinutes));

        Title = title.Trim();
        DurationMinutes = durationMinutes;
    }

    public string Title { get; }
    public int DurationMinutes { get; }

    public abstract string Conduct();
}
