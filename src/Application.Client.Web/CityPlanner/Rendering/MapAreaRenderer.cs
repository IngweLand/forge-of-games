using System.Drawing;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class MapAreaRenderer
{
    private readonly IMapGrid _grid;
    private readonly MapStyle _mapStyle;

    private readonly MapArea _mapArea;
    private List<LayerItem> _bottomLayerItems;

    private IList<Tuple<SKPoint, SKPoint>>? _gridLines;
    private List<LayerItem> _topLayerItems;

    public MapAreaRenderer(MapArea mapArea, IMapGrid grid, MapStyle mapStyle)
    {
        _mapArea = mapArea;
        _grid = grid;
        _mapStyle = mapStyle;

        var bottomLayerExtensions = mapArea.Expansions
            .Where(e => e.Type is ExpansionType.Undefined or ExpansionType.Linked).ToList();
        _bottomLayerItems = bottomLayerExtensions.Select(CreateLayerItem).ToList();
        _topLayerItems = mapArea.Expansions.Where(e => !bottomLayerExtensions.Contains(e)).Select(CreateLayerItem)
            .ToList();

        Bounds = grid.GridToScreen(new Rectangle(_mapArea.Bounds.X, _mapArea.Bounds.Y,
            _mapArea.Bounds.Width,
            _mapArea.Bounds.Height));
    }

    public Rectangle Bounds { get; }

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

        if (_gridLines == null)
        {
            CreateGridLines();
        }

        DrawGridLines(canvas);
        foreach (var layerItem in _topLayerItems)
        {
            canvas.DrawRect(layerItem.Rect, layerItem.Paint);
        }
    }

    private LayerItem CreateLayerItem(Expansion expansion)
    {
        return new LayerItem()
        {
            Rect = _grid.GridToScreen(new Rectangle(expansion.X, expansion.Y,
                _mapArea.ExpansionSize,
                _mapArea.ExpansionSize)).ToSKRect(),
            Paint = GetPaint(expansion),
        };
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
