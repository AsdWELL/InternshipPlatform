using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Company.Logo
{
    public class InvalidLogoTypeException(IEnumerable<string> extensions) 
        : UnsupportedMediaTypeException($"Недопустимый формат логотипа компанини." +
            $" Допустимы {string.Join(',', extensions)}");
}
