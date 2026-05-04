using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentGroupApplication
{
    public class StudentAlreadyHasGroupApplicationException() : ConflictException("InviteCode", "У вас уже создан запрос на добавление в другую группу");
}
