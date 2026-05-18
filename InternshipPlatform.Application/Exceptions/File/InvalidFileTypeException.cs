using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.File
{
    public class InvalidFileTypeException(IEnumerable<string> extensions) 
        : UnsupportedMediaTypeException($"Недопустимый формат файла." +
            $" Допустимы {string.Join(',', extensions)}");
}
