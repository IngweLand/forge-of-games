using System.IO.Hashing;
using System.Security.Cryptography;
using System.Text;
using Ingweland.Fog.Application.Core.Interfaces;

namespace Ingweland.Fog.Application.Core.Utils;

public class CityExpansionsHasher : ICityExpansionsHasher
{
    public string Compute(IEnumerable<string> unlockedExpansions)
    {
        var trimmed = unlockedExpansions.Select(x => x.Trim()).Order().ToList();
        var sb = new StringBuilder();
        foreach (var s in trimmed)
        {
            sb.Append(s);
        }

        var inputBytes = Encoding.UTF8.GetBytes(sb.ToString());

        return XxHash3.HashToUInt64(inputBytes).ToString("X16");
    }
}
