namespace InternshipPlatform.Application.Interfaces.Services
{
    public interface IInviteCodeGenerator
    {
        Task<string> GenerateAsync();
    }
}
