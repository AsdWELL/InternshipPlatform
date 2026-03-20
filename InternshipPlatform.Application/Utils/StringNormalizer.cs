namespace InternshipPlatform.Application.Utils
{
    public static class StringNormalizer
    {
        public static string? NormalizeOptional(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.Trim();
        }

        public static string NormalizeRequired(string value)
        {
            return value.Trim();
        }

        public static string? NormalizeToLower(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            return value.Trim().ToLowerInvariant();
        }

        public static string? NormalizeName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            var trimmedValue = name.Trim().ToLowerInvariant();

            return char.ToUpperInvariant(trimmedValue[0]) + trimmedValue[1..];
        }

        public static string? NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            var digits = new string([.. phone.Where(char.IsDigit)]);

            if (digits.Length == 11 && digits.StartsWith('8'))
                digits = "7" + digits[1..];

            if (digits.Length == 11 && digits.StartsWith('7'))
                return "+" + digits;

            return null;
        }
    }
}
