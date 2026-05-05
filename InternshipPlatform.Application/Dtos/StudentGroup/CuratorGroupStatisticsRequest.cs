using System.Text.Json.Serialization;

namespace InternshipPlatform.Application.Dtos.StudentGroup
{
    public class CuratorGroupStatisticsRequest
    {
        [JsonIgnore]
        public int CuratorId { get; set; } 
        
        public int GroupId { get; set; } 
        
        public int StudentId { get; set; }
    }
}
