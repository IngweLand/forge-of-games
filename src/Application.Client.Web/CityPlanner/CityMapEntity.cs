using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner;

public class CityMapEntity
{
    private bool _isRotated;
    private bool _isSelected;
    private Point _location;

    public CityMapEntity(int id, Point location, Size size, string name, string cityEntityId, int level,
        BuildingType buildingType, BuildingGroup buildingGroup, ExpansionSubType expansionSubType,
        int overflowRange = -1, bool isMovable = true)
    {
        Id = id;
        Location = location;
        Size = size;
        LastValidLocation = location;
        CityEntityId = cityEntityId;
        Level = level;
        BuildingType = buildingType;
        BuildingGroup = buildingGroup;
        ExpansionSubType = expansionSubType;
        Name = name;
        OverflowRange = overflowRange;
        UpdateBounds();
        IsMovable = isMovable;
    }

    public Rectangle Bounds { get; private set; }
    public BuildingGroup BuildingGroup { get; }
    public BuildingType BuildingType { get; }
    public bool CanBePlaced { get; set; } = true;
    public string CityEntityId { get; }
    public string? CustomizationId { get; set; }

    public bool ExcludeFromStats { get; set; }
    public ExpansionSubType ExpansionSubType { get; }
    public ExpansionType ExpansionType { get; }

    public float HappinessFraction { get; set; } = -1;

    public int Id { get; set; }

    public bool IsMovable { get; init; } = true;

    public bool IsRotated
    {
        get => _isRotated;
        set
        {
            if (_isRotated == value)
            {
                return;
            }

            _isRotated = value;
            UpdateBounds();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if (_isSelected)
            {
                LastValidLocation = _location;
                LastValidRotation = _isRotated;
            }
        }
    }

    public Point LastValidLocation { get; private set; }
    public bool LastValidRotation { get; private set; }

    public int Level { get; }

    public Point Location
    {
        get => _location;
        set
        {
            if (_location == value)
            {
                return;
            }

            _location = value;
            UpdateBounds();
        }
    }

    public string Name { get; }

    public Rectangle? OverflowBounds { get; private set; }

    public int OverflowRange { get; }

    public string? SelectedProductId { get; set; }

    public Size Size { get; }

    public IReadOnlyCollection<ICityMapEntityStats> Stats { get; init; } = new List<ICityMapEntityStats>();

    public CityMapEntity CloneWithLevel(string cityEntityId, int level, IReadOnlyCollection<ICityMapEntityStats> stats)
    {
        var newEntity = new CityMapEntity(Id, Location, Size, Name, cityEntityId, level, BuildingType, BuildingGroup,
            ExpansionSubType, OverflowRange, IsMovable)
        {
            Bounds = Bounds,
            CanBePlaced = CanBePlaced,
            LastValidLocation = LastValidLocation,
            LastValidRotation = LastValidRotation,
            OverflowBounds = OverflowBounds,
            _isRotated = IsRotated,
            _isSelected = IsSelected,
            _location = Location,
            SelectedProductId = SelectedProductId,
            CustomizationId = CustomizationId,
            Stats = stats,
            ExcludeFromStats = ExcludeFromStats,
        };

        return newEntity;
    }

    private void UpdateBounds()
    {
        Bounds = _isRotated
            ? new Rectangle(_location.X, _location.Y, Size.Height, Size.Width)
            : new Rectangle(_location.X, _location.Y, Size.Width, Size.Height);

        if (OverflowRange > 0)
        {
            OverflowBounds =
                new Rectangle(Bounds.X - OverflowRange, Bounds.Y - OverflowRange,
                    Bounds.Width + OverflowRange * 2, Bounds.Height + OverflowRange * 2);
        }
    }
}
