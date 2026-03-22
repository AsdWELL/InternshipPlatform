using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.Company
{
    public class CompanyNotFoundException() : NotFoundException("Компания не найдена");
}
