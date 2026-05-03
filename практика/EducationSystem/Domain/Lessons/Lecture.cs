namespace Domain.Lessons;

public sealed class Lecture : Lesson
{
    public Lecture(string title, int durationMinutes) : base(title, durationMinutes)
    {
    }

    public override string Conduct()
    {
        return $"Лекція: {Title}, тривалість {DurationMinutes} хв.";
    }
}
