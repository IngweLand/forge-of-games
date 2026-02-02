using System.Text;
using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Rendering;

public static class SkiaTextUtils
{
    private const string ELLIPSIS = "…";

    public static void DrawMultilineText(
        SKCanvas canvas,
        string text,
        SKRect bounds,
        float padding,
        SKFont font,
        SKPaint paint)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        // Create working rectangle with padding
        var workingRect = new SKRect(
            bounds.Left + padding,
            bounds.Top + padding,
            bounds.Right - padding,
            bounds.Bottom - padding
        );

        // Get font metrics
        var fontMetrics = font.Metrics;
        var lineHeight = fontMetrics.Bottom - fontMetrics.Top;
        var lineSpacing = lineHeight * 1.2f; // Add 20% for comfortable reading

        // Split text into words
        var words = text.Split(' ');
        var lines = new List<string>();
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            var testLine = currentLine.Length == 0
                ? word
                : currentLine + " " + word;

            var width = font.MeasureText(testLine, paint);

            if (width <= workingRect.Width)
            {
                currentLine.Append(currentLine.Length == 0 ? word : " " + word);
            }
            else
            {
                if (currentLine.Length > 0)
                {
                    lines.Add(currentLine.ToString());
                    currentLine.Clear();
                }

                currentLine.Append(word);
            }
        }

        if (currentLine.Length > 0)
        {
            lines.Add(currentLine.ToString());
        }

        // Draw each line
        var currentY = workingRect.Top - fontMetrics.Ascent; // Start at first baseline
        foreach (var line in lines)
        {
            // Check if we've exceeded the bottom boundary
            if (currentY + fontMetrics.Descent > workingRect.Bottom)
            {
                break;
            }

            canvas.DrawText(line, workingRect.Left, currentY, font, paint);
            currentY += lineSpacing;
        }
    }

    public static void DrawText(SKCanvas canvas, string text, SKRect bounds, SKFont font, SKPaint paint)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        // Calculate vertical position for center alignment
        var fontMetrics = font.Metrics;
        var textHeight = fontMetrics.Bottom - fontMetrics.Top;
        var centerY = bounds.MidY;
        var baselineY = centerY + textHeight / 2 - fontMetrics.Descent;

        canvas.DrawText(text, bounds.Left, baselineY, font, paint);
    }

    public static void DrawText(
        SKCanvas canvas,
        string text,
        SKRect bounds,
        float padding,
        SKFont font,
        SKPaint paint,
        TextHorizontalAlignment horizontalAlignment = TextHorizontalAlignment.Left,
        TextVerticalAlignment verticalAlignment = TextVerticalAlignment.Top)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }

        const string ELLIPSIS = "...";

        // Create working rectangle with padding
        var workingRect = new SKRect(
            bounds.Left + padding,
            bounds.Top + padding,
            bounds.Right - padding,
            bounds.Bottom - padding
        );

        // Get font metrics
        var fontMetrics = font.Metrics;
        var ellipsisWidth = font.MeasureText(ELLIPSIS, paint);
        var textWidth = font.MeasureText(text, paint);

        // Calculate vertical position based on alignment
        float baselineY;
        switch (verticalAlignment)
        {
            case TextVerticalAlignment.Top:
                baselineY = workingRect.Top - fontMetrics.Ascent;
                break;
            case TextVerticalAlignment.Center:
                var textHeight = fontMetrics.Bottom - fontMetrics.Top;
                var centerY = workingRect.MidY;
                baselineY = centerY + textHeight / 2 - fontMetrics.Descent;
                break;
            case TextVerticalAlignment.Bottom:
                baselineY = workingRect.Bottom - fontMetrics.Descent;
                break;
            default:
                baselineY = workingRect.Top - fontMetrics.Ascent;
                break;
        }

        // Check if truncation is needed
        string finalText;
        float finalX;

        if (textWidth <= workingRect.Width)
        {
            // Text fits, use it as is
            finalText = text;
        }
        else
        {
            // Binary search for the maximum number of characters that will fit
            var low = 0;
            var high = text.Length;
            var bestFit = 0;

            while (low <= high)
            {
                var mid = (low + high) / 2;
                var truncated = text.Substring(0, mid);
                var width = font.MeasureText(truncated, paint) + ellipsisWidth;

                if (width <= workingRect.Width)
                {
                    bestFit = mid;
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            // Create final truncated text with ellipsis
            finalText = text.Substring(0, bestFit).TrimEnd() + ELLIPSIS;
        }

        // Calculate horizontal position based on alignment
        var finalWidth = font.MeasureText(finalText, paint);
        switch (horizontalAlignment)
        {
            case TextHorizontalAlignment.Left:
                finalX = workingRect.Left;
                break;
            case TextHorizontalAlignment.Center:
                finalX = workingRect.MidX - finalWidth / 2;
                break;
            case TextHorizontalAlignment.Right:
                finalX = workingRect.Right - finalWidth;
                break;
            default:
                finalX = workingRect.Left;
                break;
        }

        // Draw the text
        canvas.DrawText(finalText, finalX, baselineY, font, paint);
    }
}
