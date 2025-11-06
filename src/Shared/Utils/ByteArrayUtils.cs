namespace Ingweland.Fog.Shared.Utils;

public static class ByteArrayUtils
{
    public static bool AreBytesEqual(byte[]? a, byte[]? b)
    {
        return a != null && b != null && a.Length == b.Length && ((ReadOnlySpan<byte>) a).SequenceEqual(b);
    }
}
