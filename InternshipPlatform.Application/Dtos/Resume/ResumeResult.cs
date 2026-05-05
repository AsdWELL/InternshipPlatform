namespace InternshipPlatform.Application.Dtos.Resume
{
    public class ResumeResult
    {
        public Domain.Entities.Resume Resume { get; set; }

        public int ApplicationsCount { get; set; }

        public int ViewsCount { get; set; }
    }
}
