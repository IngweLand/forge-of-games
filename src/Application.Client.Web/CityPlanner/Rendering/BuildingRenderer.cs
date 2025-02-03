using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class BuildingRenderer : IBuildingRenderer
{
    private readonly ICityMapEntityStyle _cityMapEntityStyle;
    private readonly CityPlannerSettings _settings;
    private readonly IMapGrid _grid;
    private readonly IMapTransformationComponent _mapTransformationComponent;

    private SKFont _currentNameFont;

    private SKFont _defaultNameFont;
    private SKPaint _fillPaint;
    private SKTypeface _notoSansTypeface;
    private SKPaint _strokePaint;

    public BuildingRenderer(IMapGrid grid, IMapTransformationComponent mapTransformationComponent,
        ICityMapEntityStyle cityMapEntityStyle, CityPlannerSettings settings)
    {
        _grid = grid;
        _mapTransformationComponent = mapTransformationComponent;
        _cityMapEntityStyle = cityMapEntityStyle;
        _settings = settings;
        _fillPaint = _cityMapEntityStyle.DefaultFillPaint;
        _strokePaint = _cityMapEntityStyle.DefaultStrokePaint;

        using var fontStream = typeof(BuildingRenderer).Assembly
            .GetManifestResourceStream("Ingweland.Fog.Application.Client.Web.resources.fonts.NotoSans-Regular.ttf");
        _notoSansTypeface = SKTypeface.FromStream(fontStream);
        _defaultNameFont = new SKFont(_notoSansTypeface, _cityMapEntityStyle.NameDefaultTextSize);
        _currentNameFont = _defaultNameFont;
    }
    
    private void DrawCustomization(SKRect rect, SKCanvas canvas)
    {
        var triangleSize = _grid.GridSize / 2;
        using var path = new SKPath();
        path.MoveTo(rect.Right, rect.Top);
        path.LineTo(rect.Right, rect.Top + triangleSize);
        path.LineTo(rect.Right - triangleSize, rect.Top);
        path.Close();
        canvas.DrawPath(path, _cityMapEntityStyle.CustomizationFillPaint);
    }

    public void RenderBuildings(SKCanvas canvas, IReadOnlyList<CityMapEntity> entities)
    {
        _fillPaint = _cityMapEntityStyle.DefaultFillPaint;
        _strokePaint = _cityMapEntityStyle.DefaultStrokePaint;
        var nameTextSize = (int) MathF.Min(
            MathF.Round(_cityMapEntityStyle.NameDefaultTextSize / _mapTransformationComponent.Scale),
            _cityMapEntityStyle.NameDefaultTextSize);
        if (_currentNameFont.Size != nameTextSize)
        {
            _currentNameFont = new SKFont(_notoSansTypeface, nameTextSize);
        }

        var selectedEntities = new List<CityMapEntity>();
        foreach (var entity in entities)
        {
            if (entity.IsSelected)
            {
                selectedEntities.Add(entity);
                continue;
            }

            RenderBuilding(canvas, entity);
        }

        foreach (var entity in selectedEntities)
        {
            RenderBuilding(canvas, entity);
        }
    }

    public void RenderBuilding(SKCanvas canvas, CityMapEntity entity)
    {
        RenderOverflow(canvas, entity);

        var rect = _grid.GridToScreen(entity.Bounds).ToSKRect();
        if (!entity.IsSelected)
        {
            var paint = _cityMapEntityStyle.GetPaint(entity.BuildingType);
            _fillPaint = paint;
            _strokePaint = _cityMapEntityStyle.DefaultStrokePaint;
        }
        else
        {
            if (entity.CanBePlaced)
            {
                _fillPaint = _cityMapEntityStyle.PositiveSelectionFillPaint;
                _strokePaint = _cityMapEntityStyle.PositiveSelectionStrokePaint;
            }
            else
            {
                _fillPaint = _cityMapEntityStyle.NegativeSelectionFillPaint;
                _strokePaint = _cityMapEntityStyle.NegativeSelectionStrokePaint;
            }
        }

        canvas.DrawRect(rect, _fillPaint);
        canvas.DrawRect(rect, _strokePaint);
        
        // entity name
        if (_settings.ShowEntityName && entity.Bounds.Width > 1 && entity.BuildingType != BuildingType.CultureSite)
        {
            SkiaTextUtils.DrawText(canvas, entity.Name, rect, 5, _currentNameFont, _cityMapEntityStyle.NameTextPaint);
        }

        // entity level
        if (_settings.ShowEntityLevel && (entity.BuildingType == BuildingType.CultureSite || entity.Bounds is {Width: > 1, Height: > 1}))
        {
            SkiaTextUtils.DrawText(canvas, entity.Level.ToString(), rect, 5, _currentNameFont,
                _cityMapEntityStyle.NameTextPaint, TextHorizontalAlignment.Left, TextVerticalAlignment.Bottom);
        }
        
        // buff
        if (entity is {HappinessFraction: >= 0, Bounds.Height: > 1})
        {
                var buffEntityRect = new Rectangle(entity.Bounds.Right - 1,  entity.Bounds.Bottom - 1, 1, 1);
                var buffRect = _grid.GridToScreen(buffEntityRect);
                buffRect.Offset(2, 2);
                buffRect.Inflate(-4, -4);
                DrawBuffLevel(entity.HappinessFraction, buffRect.ToSKRect(), canvas);
        }
        
        // customization
        if (entity.CustomizationId != null)
        {
            DrawCustomization(rect, canvas);
        }
    }

    private void DrawBuffLevel(float fraction, SKRect bounds, SKCanvas canvas)
    {
        // Calculate the size and position of the circle
        var diameter = Math.Min(bounds.Width, bounds.Height);
        var radius = diameter / 2f;

        // Calculate the circle's position to center it within the bounds
        var circleX = bounds.Left + (bounds.Width - diameter) / 2f + radius;
        var circleY = bounds.Top + (bounds.Height - diameter) / 2f + radius;

        var segmentPaint = _cityMapEntityStyle.CultureFillPaint;

        if (fraction >= 1)
        {
            canvas.DrawCircle(circleX, circleY, radius, segmentPaint);
            return;
        }
        using var backgroundPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White,
            IsAntialias = true,
        };
        
        // Calculate circle bounds
        var circleBounds = new SKRect(
            circleX - radius,
            circleY - radius,
            circleX + radius,
            circleY + radius
        );
        
        canvas.DrawCircle(circleX, circleY, radius, backgroundPaint);
        // Calculate sweep angle based on percentage
        var sweepAngle = fraction * 360;

        // Create segment path
        using var path = new SKPath();
        // Move to center
        path.MoveTo(circleX, circleY);
        // Draw line to start of arc (top of circle)
        path.LineTo(circleX, circleBounds.Top);
        // Draw arc
        path.ArcTo(
            circleBounds,
            -90, 
            sweepAngle,
            false);
        // Close path back to center
        path.LineTo(circleX, circleY);

        // Draw filled segment
        canvas.DrawPath(path, segmentPaint);
    }

    private void RenderOverflow(SKCanvas canvas, CityMapEntity entity)
    {
        if (entity is {IsSelected: true, OverflowBounds: not null})
        {
            var rect = _grid.GridToScreen(entity.OverflowBounds.Value).ToSKRect();
            canvas.DrawRect(rect, _cityMapEntityStyle.OverflowPaint);
            canvas.DrawRect(rect, _cityMapEntityStyle.OverflowStrokePaint);
        }
    }

    // private void RenderOverflows(SKCanvas canvas, IEnumerable<Rectangle> rectangles)
    // {
    //     canvas.SaveLayer();
    //     foreach (var rect in rectangles)
    //     {
    //         var adjustedRect = grid.GridToScreen(rect).ToSKRect();
    //         canvas.DrawRect(adjustedRect, OverflowPaint);
    //         canvas.DrawRect(adjustedRect, OverflowStrokePaint);
    //     }
    //     canvas.Restore();
    // }
}
