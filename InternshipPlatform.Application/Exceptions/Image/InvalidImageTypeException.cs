using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Image
{
    public class InvalidImageTypeException(IEnumerable<string> extensions) 
        : UnsupportedMediaTypeException($"Недопустимый формат изображения." +
            $" Допустимы {string.Join(',', extensions)}");
}
