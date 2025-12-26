using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public class IconRenderer
{
    public enum Icon
    {
        Lock,
        Construction,
        Upgrade,
    }

    private const int ICON_SIZE = 24;
    private readonly SKPath _constructionIconPath;

    private readonly string _constructionOpenIconPathData =
        "M756-120 537-339l84-84 219 219-84 84Zm-552 0-84-84 276-276-68-68-28 28-51-51v82l-28 28-121-121 28-28h82l-50-50 142-142q20-20 43-29t47-9q24 0 47 9t43 29l-92 92 50 50-28 28 68 68 90-90q-4-11-6.5-23t-2.5-24q0-59 40.5-99.5T701-841q15 0 28.5 3t27.5 9l-99 99 72 72 99-99q7 14 9.5 27.5T841-701q0 59-40.5 99.5T701-561q-12 0-24-2t-23-7L204-120Z";

    private readonly SKPath _lockIconPath;

    private readonly string _lockIconPathData =
        "M240-80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h40v-80q0-83 58.5-141.5T480-920q83 0 141.5 58.5T680-720v80h40q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Zm0-80h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM360-640h240v-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80ZM240-160v-400 400Z";

    private readonly SKPath _upgradeIconPath;

    private readonly string _upgradeIconPathData =
        "M280-160v-80h400v80H280Zm160-160v-327L336-544l-56-56 200-200 200 200-56 56-104-103v327h-80Z";

    private SKPath _lockOpenIconPath;

    private string _lockOpenIconPathData =
        "M240-160h480v-400H240v400Zm240-120q33 0 56.5-23.5T560-360q0-33-23.5-56.5T480-440q-33 0-56.5 23.5T400-360q0 33 23.5 56.5T480-280ZM240-160v-400 400Zm0 80q-33 0-56.5-23.5T160-160v-400q0-33 23.5-56.5T240-640h280v-80q0-83 58.5-141.5T720-920q83 0 141.5 58.5T920-720h-80q0-50-35-85t-85-35q-50 0-85 35t-35 85v80h120q33 0 56.5 23.5T800-560v400q0 33-23.5 56.5T720-80H240Z";

    public IconRenderer()
    {
        var viewBox = new SKRect(0, 0, 960, 0);
        _lockIconPath = NormalizeSvgPath(_lockIconPathData, viewBox, ICON_SIZE);
        _lockOpenIconPath = NormalizeSvgPath(_lockIconPathData, viewBox, ICON_SIZE);
        _lockOpenIconPath = NormalizeSvgPath(_constructionOpenIconPathData, viewBox, ICON_SIZE);
        _upgradeIconPath = NormalizeSvgPath(_upgradeIconPathData, viewBox, ICON_SIZE);
    }

    private static SKPath NormalizeSvgPath(string pathData, SKRect viewBox, float targetSize)
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

    public void DrawLockIcon(SKCanvas canvas, SKRect bounds, Icon icon, SKPaint iconPaint)
    {
        canvas.Save();

        var offsetX = bounds.MidX - ICON_SIZE / 2f;
        var offsetY = bounds.MidY + ICON_SIZE / 2f;

        canvas.Translate(offsetX, offsetY);
        var iconPath = icon switch
        {
            Icon.Lock => _lockIconPath,
            Icon.Construction => _constructionIconPath,
            Icon.Upgrade => _upgradeIconPath,
        };
        canvas.DrawPath(iconPath, iconPaint);

        canvas.Restore();
    }
}
