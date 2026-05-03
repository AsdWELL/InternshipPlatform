namespace InternshipPlatform.Application.Dtos.StudentGroup
{
    public class StudentGroupResult
    {
        public Domain.Entities.StudentGroup StudentGroup { get; set; }

        public int StudentsCount { get; set; }
    }
}
