using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentGroupApplication
{
    public class StudentAlreadyHasGroupException() : ConflictException("InviteCode", "Вы уже состоите в группе");
}
