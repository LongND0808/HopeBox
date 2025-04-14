namespace HopeBox.Common.Constant
{
    public class Constant
    {
        public const string LowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        public const string UppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string SpecialChars = "!@#$%^&*()_-+=<>?";
        public const string DigitChars = "0123456789";
        public const string AllChars = LowercaseChars + UppercaseChars + SpecialChars + DigitChars;
        public const string PasswordRegexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
    }
}
