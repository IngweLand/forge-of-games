using System.IO.Hashing;
using System.Text;
using Ingweland.Fog.Application.Server.Interfaces;

namespace Ingweland.Fog.Application.Server.Utils;

public class CityExpansionsHasher : ICityExpansionsHasher
{
    public ulong Compute(IEnumerable<string> unlockedExpansions)
    {
        var sb = new StringBuilder();
        foreach (var s in unlockedExpansions.Order())
        {
            sb.Append(s);
        }

        var inputBytes = Encoding.UTF8.GetBytes(sb.ToString());
        Span<byte> hashBytes = stackalloc byte[8];

        XxHash64.Hash(inputBytes, hashBytes);

        return BitConverter.ToUInt64(hashBytes);
    }
}
