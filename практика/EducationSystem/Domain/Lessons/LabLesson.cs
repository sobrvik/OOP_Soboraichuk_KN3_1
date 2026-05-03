namespace Domain.Lessons;

public sealed class LabLesson : Lesson
{
    public LabLesson(string title, int durationMinutes) : base(title, durationMinutes)
    {
    }

    public override string Conduct()
    {
        return $"Лабораторна робота: {Title}, тривалість {DurationMinutes} хв.";
    }
}
