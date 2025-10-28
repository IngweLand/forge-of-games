using Ingweland.Fog.Shared.Utils;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Ingweland.Fog.Infrastructure.Converters;

public class BytearrayCompressionConverter()
    : ValueConverter<byte[], byte[]>(v => Compress(v), v => Decompress(v))
{
    private static byte[] Compress(byte[] text)
    {
        return CompressionUtils.Compress(text);
    }

    private static byte[] Decompress(byte[] compressedBytes)
    {
        return CompressionUtils.Decompress(compressedBytes);
    }
}

public class ByteArrayValueComparer() : ValueComparer<byte[]>((b1, b2) => b1!.SequenceEqual(b2!),
    b => b.Aggregate(0, HashCode.Combine), b => b.ToArray());