using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeApplication
{
    public class ActivePracticeApplicationAlreadyExistsException() 
        : ConflictException("PracticeApplication", "У вас уже есть активная заявка на практику");
}
