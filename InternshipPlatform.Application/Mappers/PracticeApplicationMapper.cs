using InternshipPlatform.Application.Dtos.PracticeApplication;
using InternshipPlatform.Domain.Entities;

namespace InternshipPlatform.Application.Mappers
{
    public static class PracticeApplicationMapper
    {
        public static PracticeApplication ToDomain(
            this CreatePracticeApplicationRequest request,
            PracticePeriod practicePeriod)
        {
            return new PracticeApplication
            {
                StudentId = request.StudentId,
                PracticePeriodId = practicePeriod.Id,
                PracticeOfferId = request.PracticeOfferId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public static StudentPracticeApplicationItem ToStudentItem(this PracticeApplicationResult result)
        {
            var application = result.PracticeApplication;
            
            return new StudentPracticeApplicationItem
            {
                Id = application.Id,
                PracticePeriodId = application.PracticePeriodId,
                PracticeOfferId = application.PracticeOfferId,
                PracticeOfferTitle = application.PracticeOffer.Title,
                AvailablePlacesCount = result.AvailablePlaces,
                CompanyId = application.PracticeOffer.CompanyId,
                CompanyName = application.PracticeOffer.Company.Name,
                CompanyLogoPath = application.PracticeOffer.Company.LogoPath,
                AcademicYearStart = application.PracticePeriod.AcademicYearStart,
                CourseNumber = application.PracticePeriod.CourseNumber,
                PracticeStartDate = application.PracticePeriod.StartDate,
                PracticeEndDate = application.PracticePeriod.EndDate,
                CreatedAt = application.CreatedAt
            };
        }

        public static EmployerPracticeApplicationItem ToEmployerItem(this PracticeApplicationResult result)
        {
            var application = result.PracticeApplication;
            
            return new EmployerPracticeApplicationItem
            {
                Id = application.Id,
                StudentId = application.StudentId,
                StudentName = application.Student.Name,
                StudentSurname = application.Student.Surname,
                StudentPatronymic = application.Student.Patronymic,
                StudentAvatarPath = application.Student.AvatarPath,
                PracticePeriodId = application.PracticePeriodId,
                PracticeOfferId = application.PracticeOfferId,
                PracticeOfferTitle = application.PracticeOffer.Title,
                AvailablePlacesCount = result.AvailablePlaces,
                AcademicYearStart = application.PracticePeriod.AcademicYearStart,
                CourseNumber = application.PracticePeriod.CourseNumber,
                PracticeStartDate = application.PracticePeriod.StartDate,
                PracticeEndDate = application.PracticePeriod.EndDate,
                CreatedAt = application.CreatedAt
            };
        }

        public static EmployerPracticeApplicationDetails ToEmployerDetails(this PracticeApplicationResult result)
        {
            var application = result.PracticeApplication;
            
            return new EmployerPracticeApplicationDetails
            {
                Id = application.Id,
                StudentId = application.StudentId,
                StudentName = application.Student.Name,
                StudentSurname = application.Student.Surname,
                StudentPatronymic = application.Student.Patronymic,
                StudentAvatarPath = application.Student.AvatarPath,
                StudentEmail = application.Student.User.Email,
                StudentPhone = application.Student.Phone,
                GithubLink = application.Student.GithubLink,
                GroupId = application.Student.Group!.Id,
                GroupName = application.Student.Group.Name,
                PracticePeriodId = application.PracticePeriodId,
                PracticeOfferId = application.PracticeOfferId,
                PracticeOfferTitle = application.PracticeOffer.Title,
                AvailablePlacesCount = result.AvailablePlaces,
                AcademicYearStart = application.PracticePeriod.AcademicYearStart,
                CourseNumber = application.PracticePeriod.CourseNumber,
                PracticeStartDate = application.PracticePeriod.StartDate,
                PracticeEndDate = application.PracticePeriod.EndDate,
                CreatedAt = application.CreatedAt
            };
        }
    }
}
