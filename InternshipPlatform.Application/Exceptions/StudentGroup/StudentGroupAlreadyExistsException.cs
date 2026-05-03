using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentGroup
{
    public class StudentGroupAlreadyExistsException(string groupName) : ConflictException("Name", $"У вас уже существует группа {groupName}");
}
