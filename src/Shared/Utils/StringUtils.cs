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

    private static readonly RomanNumeral[] Numerals =
    [
        new(1000, "M"),
        new(900, "CM"),
        new(500, "D"),
        new(400, "CD"),
        new(100, "C"),
        new(90, "XC"),
        new(50, "L"),
        new(40, "XL"),
        new(10, "X"),
        new(9, "IX"),
        new(5, "V"),
        new(4, "IV"),
        new(1, "I"),
    ];

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

    public static string ToRomanNumeral(int number)
    {
        if (number is <= 0 or > 3999)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Value must be in the range 1 - 3999");
        }

        var result = string.Empty;

        foreach (var n in Numerals)
        {
            while (number >= n.Value)
            {
                result += n.Symbol;
                number -= n.Value;
            }
        }

        return result;
    }

    private readonly struct RomanNumeral(int value, string symbol)
    {
        public int Value { get; } = value;
        public string Symbol { get; } = symbol;
    }
}
