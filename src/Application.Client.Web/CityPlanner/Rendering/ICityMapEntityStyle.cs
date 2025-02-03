using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public interface ICityMapEntityStyle
{
    SKPaint CultureFillPaint { get; }
    SKPaint DefaultFillPaint { get; }
    SKPaint DefaultStrokePaint { get; }
    int NameDefaultTextSize { get; }
    SKPaint NameTextPaint { get; }
    SKPaint NegativeSelectionFillPaint { get; }
    SKPaint NegativeSelectionStrokePaint { get; }
    SKPaint OverflowPaint { get; }
    SKPaint OverflowStrokePaint { get; }
    SKPaint PositiveSelectionFillPaint { get; }
    SKPaint CustomizationFillPaint { get; }
    SKPaint PositiveSelectionStrokePaint { get; }
    SKPaint GetPaint(BuildingType buildingType);
}
