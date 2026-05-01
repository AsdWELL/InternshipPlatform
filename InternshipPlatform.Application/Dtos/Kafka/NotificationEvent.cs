namespace InternshipPlatform.Application.Dtos.Kafka
{
    public static class NotificationTitles
    {
        
    }
    
    public class NotificationEvent
    {
        public string Email { get; set; }
        
        public string Title { get; set; }
        
        public string Message { get; set; }
    }
}
