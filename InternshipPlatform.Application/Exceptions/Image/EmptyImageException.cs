using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Image
{
    public class EmptyImageException() : BadRequestException("Загружен пустой файл");
}
