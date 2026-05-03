namespace Domain.Services;

public sealed class UniversityStateDto
{
    public string FacultyName { get; set; } = string.Empty;
    public List<GroupDto> Groups { get; set; } = new();
}

public sealed class GroupDto
{
    public string Name { get; set; } = string.Empty;
    public List<StudentDto> Students { get; set; } = new();
}

public sealed class StudentDto
{
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }
}
