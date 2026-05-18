using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeMaterial
{
    public class PracticeMaterialNotFoundException() : NotFoundException("Материал практики не найден");
}
