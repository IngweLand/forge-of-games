using System.Security.Cryptography;
using System.Text;

namespace Ingweland.Fog.Shared.Utils;

public class HashUtils
{
    public static string GetMd5Hash(string input)
    {
        var sBuilder = new StringBuilder();
        var data = MD5.HashData(Encoding.UTF8.GetBytes(input));
        foreach (var t in data)
        {
            sBuilder.Append(t.ToString("x2"));
        }

        return sBuilder.ToString();
    }

    public static byte[] GetMd5HashBytes(string input)
    {
        return MD5.HashData(Encoding.UTF8.GetBytes(input));
    }
}
