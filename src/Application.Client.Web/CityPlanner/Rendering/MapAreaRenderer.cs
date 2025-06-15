using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class MapAreaRenderer
{
    private const int LockIconSize = 24;
    private readonly IMapGrid _grid;

    private readonly MapArea _mapArea;
    private readonly MapStyle _mapStyle;
    private List<LayerItem> _bottomLayerItems;

    private IList<Tuple<SKPoint, SKPoint>>? _gridLines;
    private SKPath _lockIconPath;

    private string _lockIconPathData =
        "M240-80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h40v-80q0-83 58.5-141.5T480-920q83 0 141.5 58.5T680-720v80h40q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Zm0-80h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM360-640h240v-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80ZM240-160v-400 400Z";

    private SKPath _lockOpenIconPath;

    private string _lockOpenIconPathData =
        "M240-160h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM240-160v-400 400Zm0 80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h280v-80q0-83 58.5-141.5T720-920q83 0 141.5 58.5T920-720h-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80h120q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Z";

    private List<LayerItem> _topLayerItems;

    public MapAreaRenderer(MapArea mapArea, IMapGrid grid, MapStyle mapStyle)
    {
        _mapArea = mapArea;
        _grid = grid;
        _mapStyle = mapStyle;

        var viewBox = new SKRect(0, 0, 960, 0);
        _lockIconPath = NormalizeSvgPath(_lockIconPathData, viewBox, LockIconSize);
        _lockOpenIconPath = NormalizeSvgPath(_lockIconPathData, viewBox, LockIconSize);

        var bottomLayerExpansions = mapArea.Expansions
            .Where(e => e.Type is ExpansionType.Undefined or ExpansionType.Linked).ToList();
        _bottomLayerItems = bottomLayerExpansions.Select(CreateLayerItem).ToList();
        _bottomLayerItems.AddRange(CreateHappinessTiles());
        _topLayerItems = mapArea.Expansions.Where(e => !bottomLayerExpansions.Contains(e)).Select(CreateLayerItem)
            .ToList();

        Bounds = grid.GridToScreen(new Rectangle(_mapArea.Bounds.X, _mapArea.Bounds.Y,
            _mapArea.Bounds.Width,
            _mapArea.Bounds.Height));
    }

    public Rectangle Bounds { get; }

    private List<LayerItem> CreateHappinessTiles()
    {
        return _mapArea.MapAreaHappinessProviders.Select(src => new LayerItem()
        {
            Rect = _grid.GridToScreen(src.Bounds).ToSKRect(),
            Paint = _mapStyle.HappinessPaint
        }).ToList();
    }

    private SKPaint GetPaint(Expansion expansion)
    {
        return expansion.SubType == ExpansionSubType.Water
            ? _mapStyle.ExpansionSubPaints[ExpansionSubType.Water]
            : _mapStyle.ExpansionPaints[expansion.Type];
    }

    private void CreateGridLines()
    {
        _gridLines = new List<Tuple<SKPoint, SKPoint>>();
        for (var j = Bounds.Y; j < Bounds.Bottom + 1; j += _grid.GridSize)
        {
            _gridLines.Add(new Tuple<SKPoint, SKPoint>(new SKPoint(Bounds.X, j),
                new SKPoint(Bounds.Right, j)));
        }

        for (var j = Bounds.X; j < Bounds.Right + 1; j += _grid.GridSize)
        {
            _gridLines.Add(new Tuple<SKPoint, SKPoint>(new SKPoint(j, Bounds.Y),
                new SKPoint(j, Bounds.Bottom)));
        }
    }

    public void Render(SKCanvas canvas)
    {
        canvas.Clear(_mapStyle.BackgroundColor);

        foreach (var layerItem in _bottomLayerItems)
        {
            canvas.DrawRect(layerItem.Rect, layerItem.Paint);
        }

        DrawLockedExpansions(canvas);

        if (_gridLines == null)
        {
            CreateGridLines();
        }

        DrawGridLines(canvas);
        foreach (var layerItem in _topLayerItems)
        {
            canvas.DrawRect(layerItem.Rect, layerItem.Paint);
        }

        DrawExpansionIcons(canvas);
    }

    private LayerItem CreateLayerItem(Expansion expansion)
    {
        return new LayerItem()
        {
            Rect = _grid.GridToScreen(new Rectangle(expansion.X, expansion.Y,
                _mapArea.ExpansionSize,
                _mapArea.ExpansionSize)).ToSKRect(),
            Paint = GetPaint(expansion)
        };
    }

    private LayerItem CreateLockedLayerItem(Rectangle bounds)
    {
        return new LayerItem()
        {
            Rect = _grid.GridToScreen(bounds).ToSKRect(),
            Paint = _mapStyle.LockedExpansionPaint
        };
    }

    private void DrawLockedExpansions(SKCanvas canvas)
    {
        foreach (var expansion in _mapArea.LockedExpansions)
        {
            var layerItem = CreateLockedLayerItem(expansion.Bounds);
            canvas.DrawRect(layerItem.Rect, layerItem.Paint);
        }
    }

    private void DrawExpansionIcons(SKCanvas canvas)
    {
        foreach (var expansion in _mapArea.LockedExpansions)
        {
            canvas.Save();

            var rect = _grid.GridToScreen(expansion.Bounds).ToSKRect();
            var offsetX = rect.MidX - LockIconSize / 2f;
            var offsetY = rect.MidY + LockIconSize / 2f;

            canvas.Translate(offsetX, offsetY);
            canvas.DrawPath(_lockIconPath, _mapStyle.ExpansionIconPaint);

            canvas.Restore();
        }
    }

    private SKPath NormalizeSvgPath(string pathData, SKRect viewBox, float targetSize)
    {
        var path = SKPath.ParseSvgPathData(pathData);

        var scaleX = targetSize / viewBox.Width;
        var scaleY = targetSize / viewBox.Height;
        var scale = Math.Min(scaleX, scaleY);

        var offsetX = -viewBox.Left;
        var offsetY = -viewBox.Top;

        var matrix = SKMatrix.CreateTranslation(offsetX, offsetY);
        matrix = SKMatrix.CreateScale(scale, scale).PostConcat(matrix);

        var normalizedPath = new SKPath();
        path.Transform(matrix, normalizedPath);
        return normalizedPath;
    }

    private void DrawGridLines(SKCanvas canvas)
    {
        foreach (var line in _gridLines)
        {
            canvas.DrawLine(line.Item1, line.Item2, _mapStyle.GridLinePaint);
        }
    }

    private class LayerItem
    {
        public SKPaint Paint { get; set; }
        public SKRect Rect { get; set; }
    }
}