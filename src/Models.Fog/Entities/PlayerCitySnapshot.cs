using Ingweland.Fog.Models.Hoh.Enums;
using Ingweland.Fog.Shared.Utils;

namespace Ingweland.Fog.Models.Fog.Entities;

public class PlayerCitySnapshot
{
    private byte[] _compressedData;
    private byte[]? _data;
    public required string AgeId { get; set; }
    public required CityId CityId { get; set; }
    public int Coins { get; set; }

    public required DateOnly CollectedAt { get; set; }

    [Obsolete]
    public byte[] CompressedData
    {
        get => _compressedData;
        set
        {
            _compressedData = value;
            _data = null;
        }
    }

    [Obsolete]
    public required byte[] Data
    {
        get => _data ??= CompressionUtils.Decompress(_compressedData);
        set
        {
            _data = value;
            _compressedData = CompressionUtils.Compress(value);
        }
    }

    public required PlayerCitySnapshotDataEntity Data2 { get; set; }

    public int Food { get; set; }
    public int Goods { get; set; }

    public float HappinessUsageRatio { get; set; }
    public bool HasPremiumBuildings { get; set; }
    public int Id { get; set; }
    public required string OpenedExpansionsHash { get; set; }
    public Player Player { get; set; } = null!;

    public int PlayerId { get; set; }
    public required int TotalArea { get; set; }
}
