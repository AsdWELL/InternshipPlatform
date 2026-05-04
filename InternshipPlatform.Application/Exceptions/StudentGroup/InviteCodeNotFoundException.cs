using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.StudentGroup
{
    public class InviteCodeNotFoundException() : ConflictException("InviteCode", "Код приглашения в группу не найден");
}
