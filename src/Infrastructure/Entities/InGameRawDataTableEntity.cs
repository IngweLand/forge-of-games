using System.Runtime.Serialization;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Infrastructure.Entities;

public class InGameRawDataTableEntity : InGameBinDataTableEntityBase
{
    private const int MaxRequestSegmentsCount = 4;
    private const int RequestSegmentSizeBytes = 60 * 1000;

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

    public byte[]? CompressedRequestData { get; set; }
    public byte[]? CompressedRequestData1 { get; set; }
    public byte[]? CompressedRequestData2 { get; set; }
    public byte[]? CompressedRequestData3 { get; set; }

    [IgnoreDataMember]
    public string? RequestBase64Data
    {
        get
        {
            var combined = CombineRequestSegments();
            return combined.Length == 0 ? null : CompressionUtils.DecompressToString(combined);
        }
        set
        {
            if (value == null)
            {
                return;
            }

            var compressed = CompressionUtils.CompressString(value);
            SplitRequestIntoSegments(compressed);
        }
    }

    public Guid? SubmissionId { get; init; }

    private void SplitRequestIntoSegments(byte[] compressed)
    {
        var chunks = compressed.Chunk(RequestSegmentSizeBytes).ToArray();

        if (chunks.Length > MaxRequestSegmentsCount)
        {
            throw new ArgumentOutOfRangeException(
                $"Request data exceeded max allowed size in bytes: got {compressed.Length}, max {
                    MaxRequestSegmentsCount * RequestSegmentSizeBytes}");
        }

        for (var i = 0; i < chunks.Length; i++)
        {
            switch (i)
            {
                case 0: CompressedRequestData = chunks[i]; break;
                case 1: CompressedRequestData1 = chunks[i]; break;
                case 2: CompressedRequestData2 = chunks[i]; break;
                case 3: CompressedRequestData3 = chunks[i]; break;
            }
        }
    }

    private List<byte[]?> GetRequestSegments()
    {
        return
        [
            CompressedRequestData, CompressedRequestData1, CompressedRequestData2, CompressedRequestData3,
        ];
    }

    private byte[] CombineRequestSegments()
    {
        return GetRequestSegments()
            .Where(segment => segment != null)
            .SelectMany(segment => segment!)
            .ToArray();
    }
}
