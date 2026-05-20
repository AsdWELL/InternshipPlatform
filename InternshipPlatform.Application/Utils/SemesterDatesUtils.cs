namespace InternshipPlatform.Application.Utils
{
    public class SemesterDates
    {
        public DateOnly Start { get; set; }

        public DateOnly End { get; set; }
    }


    public static class SemesterDatesUtils
    {
        private const int AutumnSemesterMonthStart = 9;
        private const int AutumnSemesterMonthEnd = 12;

        private const int SpringSemesterMonthStart = 1;
        private const int SpringSemesterMonthEnd = 7;

        public static SemesterDates? GetCurrentSemesterDates()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var semesterStart = today.Month switch
            {
                >= AutumnSemesterMonthStart and <= AutumnSemesterMonthEnd 
                    => new DateOnly(today.Year, AutumnSemesterMonthEnd, 1),
                >= SpringSemesterMonthStart and <= SpringSemesterMonthEnd
                    => new DateOnly(today.Year, SpringSemesterMonthStart, 1),
                _ => (DateOnly?)null
            };

            var semesterEnd = today.Month switch
            {
                >= AutumnSemesterMonthStart and <= AutumnSemesterMonthEnd 
                    => new DateOnly(today.Year, AutumnSemesterMonthEnd, 31),
                >= SpringSemesterMonthStart and <= SpringSemesterMonthEnd 
                    => new DateOnly(today.Year, SpringSemesterMonthEnd, 31),
                _ => (DateOnly?)null
            };

            if (semesterStart is null || semesterEnd is null)
                return null;

            return new SemesterDates
            {
                Start = semesterStart.Value,
                End = semesterEnd.Value
            };
        }
    }
}
