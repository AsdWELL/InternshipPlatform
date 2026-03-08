namespace InternshipPlatform.Application.Dtos.Auth
{
    public class TokenOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Key { get; set; }

        public int ExpiresAfterHours { get; set; }

        public string CookieTitle { get; set; }
    }
}
