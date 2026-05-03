namespace Domain.Services;

using System.Text.Json;
using Domain.Components;

using System.Text.Encodings.Web;

public sealed class JsonUniversityStorage
{
    private readonly JsonSerializerOptions _options = new()
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

    public string SaveToJson(Faculty faculty)
    {
        var dto = ToDto(faculty);
        return JsonSerializer.Serialize(dto, _options);
    }

    public Faculty LoadFromJson(string json)
    {
        var dto = JsonSerializer.Deserialize<UniversityStateDto>(json, _options)
                  ?? throw new InvalidOperationException("Не вдалося прочитати JSON.");

        var faculty = new Faculty(dto.FacultyName);

        foreach (var groupDto in dto.Groups)
        {
            var group = new Group(groupDto.Name);

            foreach (var studentDto in groupDto.Students)
                group.Add(new Student(studentDto.Name, studentDto.Score));

            faculty.Add(group);
        }

        return faculty;
    }

    private static UniversityStateDto ToDto(Faculty faculty)
    {
        var dto = new UniversityStateDto
        {
            FacultyName = faculty.Name
        };

        foreach (var groupComponent in faculty.Children)
        {
            if (groupComponent is not Group group)
                continue;

            var groupDto = new GroupDto
            {
                Name = group.Name
            };

            foreach (var studentComponent in group.Children)
            {
                if (studentComponent is Student student)
                {
                    groupDto.Students.Add(new StudentDto
                    {
                        Name = student.Name,
                        Score = student.Score
                    });
                }
            }

            dto.Groups.Add(groupDto);
        }

        return dto;
    }
}
