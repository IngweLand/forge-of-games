using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Logging;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class BuildingRenderer : IBuildingRenderer
{
    private const int BUFF_PADDING = 2;
    private readonly ICityMapEntityStyle _cityMapEntityStyle;
    private readonly IMapGrid _grid;
    private readonly IconRenderer _iconRenderer = new();
    private readonly Task _initializationTask;
    private readonly ILogger<BuildingRenderer> _logger;
    private readonly IMapTransformationComponent _mapTransformationComponent;
    private readonly IProductionRenderer _productionRenderer;
    private readonly CityPlannerSettings _settings;
    private readonly ITypefaceProvider _typefaceProvider;
    private int _buffSize;
    private SKFont _currentNameFont;
    private SKFont _defaultNameFont;
    private SKPaint _fillPaint;
    private SKPaint _strokePaint;

    public BuildingRenderer(IMapGrid grid, IMapTransformationComponent mapTransformationComponent,
        ICityMapEntityStyle cityMapEntityStyle, CityPlannerSettings settings,
        IProductionRenderer productionRenderer, ITypefaceProvider typefaceProvider,
        ILogger<BuildingRenderer> logger)
    {
        _grid = grid;
        _mapTransformationComponent = mapTransformationComponent;
        _cityMapEntityStyle = cityMapEntityStyle;
        _settings = settings;
        _productionRenderer = productionRenderer;
        _typefaceProvider = typefaceProvider;
        _logger = logger;
        _fillPaint = _cityMapEntityStyle.DefaultFillPaint;
        _strokePaint = _cityMapEntityStyle.DefaultStrokePaint;

        _initializationTask = InitializeInternalAsync();
    }

    public Task InitializeAsync()
    {
        return _initializationTask;
    }

    public void RenderBuildings(SKCanvas canvas, IEnumerable<CityMapEntity> entities)
    {
        ThrowIfNotInitialized();

        _fillPaint = _cityMapEntityStyle.DefaultFillPaint;
        _strokePaint = _cityMapEntityStyle.DefaultStrokePaint;
        var nameTextSize = (int) MathF.Min(
            MathF.Round(_cityMapEntityStyle.NameDefaultTextSize / _mapTransformationComponent.Scale),
            _cityMapEntityStyle.NameDefaultTextSize);
        if ((int) _currentNameFont.Size != nameTextSize)
        {
            _currentNameFont = new SKFont(_typefaceProvider.MainTypeface, nameTextSize);
        }

        _productionRenderer.UpdateFontSize();

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
        ThrowIfNotInitialized();

        RenderOverflow(canvas, entity);

        var rect = _grid.GridToScreen(entity.Bounds).ToSKRect();
        if (!entity.IsSelected)
        {
            var paint = !entity.IsUnchanged
                ? _cityMapEntityStyle.GetPaint(entity.BuildingType)
                : _cityMapEntityStyle.UnchangedBuildingPaint;
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

        if (entity.IsLocked)
        {
            canvas.DrawRect(rect, _cityMapEntityStyle.LockedFillPaint);
        }

        // entity name
        if (_settings.ShowEntityName && entity.Bounds is {Width: > 1, Height: > 1} &&
            entity.BuildingType != BuildingType.CultureSite)
        {
            SkiaTextUtils.DrawText(canvas, entity.Name, rect, 5, _currentNameFont,
                !entity.IsUnchanged ? _cityMapEntityStyle.NameTextPaint : _cityMapEntityStyle.UnchangedNameTextPaint);
        }

        // entity level
        if (_settings.ShowEntityLevel)
        {
            SkiaTextUtils.DrawText(canvas, entity.Level.ToString(), rect, 5, _currentNameFont,
                !entity.IsUnchanged ? _cityMapEntityStyle.NameTextPaint : _cityMapEntityStyle.UnchangedNameTextPaint,
                TextHorizontalAlignment.Left, TextVerticalAlignment.Bottom);
        }

        // buff
        if (entity is {HappinessFraction: >= 0, Bounds.Height: > 1})
        {
            var buffRect = new SKRect(rect.Right - _buffSize - BUFF_PADDING, rect.Bottom - _buffSize - BUFF_PADDING,
                rect.Right - BUFF_PADDING, rect.Bottom - BUFF_PADDING);
            DrawBuffLevel(entity.HappinessFraction, buffRect, canvas, entity.IsUnchanged);
        }

        // customization
        if (entity.CustomizationId != null)
        {
            DrawCustomization(rect, canvas);
        }

        if (entity.IsLocked)
        {
            _iconRenderer.DrawLockIcon(canvas, rect, IconRenderer.Icon.Lock, _cityMapEntityStyle.LockIconPaint);
        }
        else if (entity.IsUpgrading)
        {
            _iconRenderer.DrawLockIcon(canvas, rect, IconRenderer.Icon.Upgrade, _cityMapEntityStyle.StateIconPaint);
        }
        else if (ProductionProviderHelper.CanSelectProduct(entity.BuildingType, entity.BuildingGroup) &&
                 entity.SelectedProduct is {Resources.Count: > 0})
        {
            _productionRenderer.Draw(canvas, rect, entity.SelectedProduct.Resources.First().Value,
                ProductionProviderHelper.CanRenderProductionLabel(entity.BuildingType)
                    ? entity.SelectedProduct.ProductionTime
                    : null,
                entity.IsUnchanged);
        }
    }

    private async Task InitializeInternalAsync()
    {
        _buffSize = (int) (_grid.GridSize * 0.5);

        await _typefaceProvider.InitializeAsync();

        _defaultNameFont = new SKFont(_typefaceProvider.MainTypeface, _cityMapEntityStyle.NameDefaultTextSize);
        _currentNameFont = _defaultNameFont;

        await _productionRenderer.InitializeAsync();
    }

    private void ThrowIfNotInitialized()
    {
        if (!_initializationTask.IsCompletedSuccessfully)
        {
            throw new InvalidOperationException($"{nameof(BuildingRenderer)} must be initialize before using.");
        }
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

    private void DrawBuffLevel(float fraction, SKRect bounds, SKCanvas canvas, bool isUnchanged)
    {
        // Calculate the size and position of the circle
        var diameter = Math.Min(bounds.Width, bounds.Height);
        var radius = diameter / 2f;

        // Calculate the circle's position to center it within the bounds
        var circleX = bounds.Left + (bounds.Width - diameter) / 2f + radius;
        var circleY = bounds.Top + (bounds.Height - diameter) / 2f + radius;

        var segmentPaint = _cityMapEntityStyle.GetBuffForegroundPaint(fraction, isUnchanged);

        if (fraction == 1.0 || fraction >= 2)
        {
            canvas.DrawCircle(circleX, circleY, radius, segmentPaint);
            return;
        }

        using var backgroundPaint = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = _cityMapEntityStyle.GetBuffBackgroundColor(fraction, isUnchanged),
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
        if (entity is {IsLocked: false, IsSelected: true, OverflowBounds: not null})
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
