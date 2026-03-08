namespace InternshipPlatform.Application.Interfaces.Services.Auth
{
    public interface IPasswordHasher
    {
        string Generate(string password);

        bool Verify(string password, string hashPassword);
    }
}
