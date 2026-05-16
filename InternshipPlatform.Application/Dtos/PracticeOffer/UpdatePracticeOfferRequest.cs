using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.PracticeOffer
{
    public class UpdatePracticeOfferRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int EmployerId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool? IsRemote { get; set; }

        public string? Region { get; set; }

        public bool? IsActive { get; set; }

        public int? MaxStudents { get; set; }

        public int? SpecializationId { get; set; }
    }
}
