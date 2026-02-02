using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public interface ICityMapEntityStyle
{
    SKPaint CultureFillPaint { get; }
    SKPaint CustomizationFillPaint { get; }
    SKPaint DefaultFillPaint { get; }
    SKPaint DefaultStrokePaint { get; }
    SKPaint LockedFillPaint { get; }
    SKPaint LockIconPaint { get; }
    int NameDefaultTextSize { get; }
    SKPaint NameTextPaint { get; }
    SKPaint NegativeSelectionFillPaint { get; }
    SKPaint NegativeSelectionStrokePaint { get; }
    SKPaint OverflowPaint { get; }
    SKPaint OverflowStrokePaint { get; }
    SKPaint PositiveSelectionFillPaint { get; }
    SKPaint PositiveSelectionStrokePaint { get; }
    int ProductionTimeDefaultTextSize { get; }
    SKPaint StateIconPaint { get; }
    SKPaint UnchangedBuildingPaint { get; }
    SKPaint UnchangedNameTextPaint { get; }
    SKPaint GetPaint(BuildingType buildingType);
    SKColor GetBuffBackgroundColor(float cultureValue, bool isUnchanged = false);
    SKPaint GetBuffForegroundPaint(float cultureValue, bool isUnchanged = false);
}
