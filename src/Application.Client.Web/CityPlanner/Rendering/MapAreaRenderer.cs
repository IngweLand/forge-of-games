using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Core.CityPlanner;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class MapAreaRenderer
{
    private readonly IMapGrid _grid;
    private readonly MapArea _mapArea;
    private readonly MapStyle _mapStyle;
    private List<LayerItem> _bottomLayerItems;
    private IList<Tuple<SKPoint, SKPoint>>? _gridLines;
    private List<LayerItem> _topLayerItems;
    private LockIconRenderer _lockIconRenderer = new();

    public MapAreaRenderer(MapArea mapArea, IMapGrid grid, MapStyle mapStyle)
    {
        _mapArea = mapArea;
        _grid = grid;
        _mapStyle = mapStyle;

        

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
            var rect = _grid.GridToScreen(expansion.Bounds).ToSKRect();
            _lockIconRenderer.DrawLockIcon(canvas, rect);
        }
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