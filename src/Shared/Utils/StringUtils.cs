using System.Text;

namespace Ingweland.Fog.Shared.Utils;

public static class StringUtils
{
    private const string ALL =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";

    private const string LOWERCASE_LETTERS = "abcdefghijklmnopqrstuvwxyz";
    private const string NUMBERS = "0123456789";
    private const string SPECIAL_CHARACTERS = "!@#$%^&*()_+-=[]{}|;:,.<>?";
    private const string UPPERCASE_LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string GenerateComplexPassword(int length = 12)
    {
        var password = new StringBuilder();

        password.Append(UPPERCASE_LETTERS[Random.Shared.Next(UPPERCASE_LETTERS.Length)]);
        password.Append(LOWERCASE_LETTERS[Random.Shared.Next(LOWERCASE_LETTERS.Length)]);
        password.Append(NUMBERS[Random.Shared.Next(NUMBERS.Length)]);
        password.Append(SPECIAL_CHARACTERS[Random.Shared.Next(SPECIAL_CHARACTERS.Length)]);

        for (var i = 4; i < length; i++)
        {
            password.Append(ALL[Random.Shared.Next(ALL.Length)]);
        }

        return password.ToString();
    }

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
    }

    public static string GetAbbreviation(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        return string.Join("",
            input.Split([' '], StringSplitOptions.RemoveEmptyEntries)
                .Select(word => word[0])
                .Select(char.ToUpper));
    }
}
