using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class LockIconRenderer
{
    private const int LockIconSize = 24;

    private readonly SKPaint _expansionIconPaint = new()
    {
        Color = SKColor.Parse("#e0e0e0"),
        IsAntialias = true,
        Style = SKPaintStyle.StrokeAndFill,
    };

    private readonly SKPath _lockIconPath;

    private readonly string _lockIconPathData =
        "M240-80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h40v-80q0-83 58.5-141.5T480-920q83 0 141.5 58.5T680-720v80h40q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Zm0-80h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM360-640h240v-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80ZM240-160v-400 400Z";

    private SKPath _lockOpenIconPath;

    private string _lockOpenIconPathData =
        "M240-160h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM240-160v-400 400Zm0 80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h280v-80q0-83 58.5-141.5T720-920q83 0 141.5 58.5T920-720h-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80h120q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Z";

    public LockIconRenderer()
    {
        var viewBox = new SKRect(0, 0, 960, 0);
        _lockIconPath = NormalizeSvgPath(_lockIconPathData, viewBox, LockIconSize);
        _lockOpenIconPath = NormalizeSvgPath(_lockIconPathData, viewBox, LockIconSize);
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

    public void DrawLockIcon(SKCanvas canvas, SKRect bounds)
    {
        canvas.Save();

        var offsetX = bounds.MidX - LockIconSize / 2f;
        var offsetY = bounds.MidY + LockIconSize / 2f;

        canvas.Translate(offsetX, offsetY);
        canvas.DrawPath(_lockIconPath, _expansionIconPaint);

        canvas.Restore();
    }
}
