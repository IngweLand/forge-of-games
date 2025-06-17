namespace Ingweland.Fog.Shared.Formatters;

public class NumberFormatter
{
    public static string FormatCompactNumber(int number)
    {
        return number switch
        {
            >= 1_000_000 => (number / 1_000_000.0).ToString("0.00") + "M",
            >= 100_000 => (number / 1000.0).ToString("0.0") + "K",
            _ => number.ToString(),
        };
    }
}
