using System.IO.Compression;
using System.Text;
using LZStringCSharp;

namespace Ingweland.Fog.Shared.Utils;

public static class CompressionUtils
{
    public static byte[] CompressString(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return [];
        }

        var textBytes = Encoding.UTF8.GetBytes(text);

        using var memoryStream = new MemoryStream();
        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
        {
            gzipStream.Write(textBytes, 0, textBytes.Length);
        }

        return memoryStream.ToArray();
    }

    public static string CompressToBase64String(string text)
    {
        var bytes = CompressString(text);
        return Convert.ToBase64String(bytes);
    }

    public static string DecompressToString(byte[] compressedData)
    {
        if (compressedData.Length == 0)
        {
            return string.Empty;
        }

        using var memoryStream = new MemoryStream(compressedData);
        using var outputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            gzipStream.CopyTo(outputStream);
        }

        return Encoding.UTF8.GetString(outputStream.ToArray());
    }

    public static string DecompressFromBase64String(string compressedText)
    {
        var compressedData = Convert.FromBase64String(compressedText);
        return DecompressToString(compressedData);
    }

    public static byte[] Compress(byte[] data)
    {
        using var memoryStream = new MemoryStream();
        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
        {
            gzipStream.Write(data, 0, data.Length);
        }

        return memoryStream.ToArray();
    }

    public static byte[] Decompress(byte[] compressedData)
    {
        if (compressedData.Length == 0)
        {
            return [];
        }

        using var memoryStream = new MemoryStream(compressedData);
        using var outputStream = new MemoryStream();
        using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
        {
            gzipStream.CopyTo(outputStream);
        }

        return outputStream.ToArray();
    }
    public static string DecompressFromLzString(string src)
    {
        return LZString.DecompressFromEncodedURIComponent(src);
    }
}
