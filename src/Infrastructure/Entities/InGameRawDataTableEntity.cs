using System.Runtime.Serialization;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameRawDataTableEntity : TableEntityBase
{
    private const int MaxSegmentsCount = 5;
    private const int SegmentSizeBytes = 60 * 1000;

    [IgnoreDataMember]
    public string Base64Data
    {
        get => CompressionUtils.DecompressToString(CombineSegments());
        set
        {
            var compressed = CompressionUtils.CompressString(value);
            SplitIntoSegments(compressed);
        }
    }

    public required DateTime CollectedAt { get; init; }

    public byte[] CompressedData { get; set; } = null!;
    public byte[]? CompressedData1 { get; set; }
    public byte[]? CompressedData2 { get; set; }
    public byte[]? CompressedData3 { get; set; }
    public byte[]? CompressedData4 { get; set; }

    private void SplitIntoSegments(byte[] compressed)
    {
        var chunks = compressed.Chunk(SegmentSizeBytes).ToArray();

        if (chunks.Length > MaxSegmentsCount)
        {
            throw new ArgumentOutOfRangeException(
                $"Data exceeded max allowed size in bytes: got {compressed.Length}, max {MaxSegmentsCount * SegmentSizeBytes}");
        }

        for (var i = 0; i < chunks.Length; i++)
        {
            switch (i)
            {
                case 0:
                {
                    CompressedData = chunks[i];
                    break;
                }
                case 1:
                {
                    CompressedData1 = chunks[i];
                    break;
                }
                case 2:
                {
                    CompressedData2 = chunks[i];
                    break;
                }
                case 3:
                {
                    CompressedData3 = chunks[i];
                    break;
                }
                case 4:
                {
                    CompressedData4 = chunks[i];
                    break;
                }
            }
        }
    }

    private List<byte[]?> GetSegments()
    {
        return [CompressedData, CompressedData1, CompressedData2, CompressedData3, CompressedData4];
    }

    private byte[] CombineSegments()
    {
        return GetSegments()
            .Where(segment => segment != null)
            .SelectMany(segment => segment!)
            .ToArray();
    }
}