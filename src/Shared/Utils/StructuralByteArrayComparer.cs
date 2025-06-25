using System.Collections;

namespace Ingweland.Fog.Shared.Utils;

public class StructuralByteArrayComparer : IEqualityComparer<byte[]>
{
    public static readonly StructuralByteArrayComparer Instance = new();

    private StructuralByteArrayComparer()
    {
    }

    public bool Equals(byte[]? x, byte[]? y)
    {
        return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
    }

    public int GetHashCode(byte[] obj)
    {
        return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
    }
}
