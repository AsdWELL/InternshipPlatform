using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Company.Logo
{
    public class EmptyLogoFileException() : BadRequestException("Загружен пустой файл логотипа");
}
