using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Image
{
    public class MaxImageSizeException(int maxSizeMb) : BadRequestException($"Максимальный размер изображения {maxSizeMb}Мб");
}
