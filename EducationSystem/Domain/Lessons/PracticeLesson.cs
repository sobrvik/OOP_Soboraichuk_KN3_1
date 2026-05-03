namespace Domain.Lessons;

public sealed class PracticeLesson : Lesson
{
    public PracticeLesson(string title, int durationMinutes) : base(title, durationMinutes)
    {
    }

    public override string Conduct()
    {
        return $"Практичне заняття: {Title}, тривалість {DurationMinutes} хв.";
    }
}
