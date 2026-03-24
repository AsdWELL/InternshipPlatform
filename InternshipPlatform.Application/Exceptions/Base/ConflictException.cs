namespace InternshipPlatform.Application.Exceptions.Base
{
    public abstract class ConflictException(string propertyName, string msg) : Exception(msg)
    {
        public string PropertyName => propertyName;
    }
}
