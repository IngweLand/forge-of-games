using Ingweland.Fog.Shared.Utils;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ingweland.Fog.Infrastructure.Converters;

public class StringCompressionConverter()
    : ValueConverter<string, byte[]>(v => Compress(v), v => Decompress(v))
{
    private static byte[] Compress(string text)
    {
        return CompressionUtils.CompressString(text);
    }

    private static string Decompress(byte[] compressedBytes)
    {
        return CompressionUtils.DecompressToString(compressedBytes);
    }
}
