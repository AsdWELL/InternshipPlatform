using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Company.Logo
{
    public class MaxLogoSizeException(int maxSizeMb) : BadRequestException($"Максимальный размер логотипа {maxSizeMb}Мб");
}
