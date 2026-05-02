using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Curator
{
    public class CuratorNotFoundException() : NotFoundException("Профиль куратора не найден");
}
