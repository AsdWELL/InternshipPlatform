namespace InternshipPlatform.Application.Utils
{
    public static class WorkExperienceUtils
    {
        public static int CalculateExperienceMonths(DateOnly startDate, DateOnly? endDate)
        {
            var actualEndDateWork = endDate
                ?? DateOnly.FromDateTime(DateTime.UtcNow);

            var workDurationMonths = (actualEndDateWork.Year - startDate.Year) * 12
                + actualEndDateWork.Month - startDate.Month;

            if (actualEndDateWork.Day < startDate.Day)
                workDurationMonths--;

            return Math.Max(workDurationMonths, 0);
        }
    }
}
