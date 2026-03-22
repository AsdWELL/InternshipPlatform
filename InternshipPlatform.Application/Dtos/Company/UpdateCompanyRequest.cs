using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.Company
{
    public class UpdateCompanyRequest
    {
        [JsonIgnore]
        public int EmployerId { get; set; }

        public string? Name { get; set; }

        public string? Inn { get; set; }

        public string? Link { get; set; }

        public string? Description { get; set; }
    }
}
