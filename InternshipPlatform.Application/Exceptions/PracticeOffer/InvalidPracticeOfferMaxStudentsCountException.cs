using InternshipPlatform.Application.Exceptions.Base;

namespace InternshipPlatform.Application.Exceptions.PracticeOffer
{
    public class InvalidPracticeOfferMaxStudentsCountException(int applicationsCount, int maxStudentCount) 
        : ConflictException("MaxStudents", $"Невозможно изменить максимальное количество студентов на практике " +
            $"на {maxStudentCount}, т.к. уже принято {applicationsCount} студентов");
}
