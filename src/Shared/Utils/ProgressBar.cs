﻿#region Author

// Author https://gist.github.com/DanielSWolf/0ab6a96899cc5377bf54

#endregion

using System.Text;

namespace Ingweland.Fog.Shared.Utils;

/// <summary>
/// An ASCII progress bar
/// </summary>
public class ProgressBar : IDisposable, IProgress<double>
{
    private const string animation = @"|/-\";
    private const int blockCount = 10;
    private readonly TimeSpan animationInterval = TimeSpan.FromSeconds(1.0 / 8);

    private readonly Timer timer;
    private int animationIndex = 0;

    private double currentProgress = 0;
    private string currentText = string.Empty;
    private bool disposed = false;

    public ProgressBar()
    {
        timer = new Timer(TimerHandler);

        // A progress bar is only for temporary display in a console window.
        // If the console output is redirected to a file, draw nothing.
        // Otherwise, we'll end up with a lot of garbage in the target file.
        if (!Console.IsOutputRedirected)
        {
            ResetTimer();
        }
    }

    public string BaseMessage { get; set; }

    public void Dispose()
    {
        lock (timer)
        {
            disposed = true;
            UpdateText(string.Empty);
        }
    }

    public void Report(double value)
    {
        // Make sure value is in [0..1] range
        value = Math.Max(0, Math.Min(1, value));
        Interlocked.Exchange(ref currentProgress, value);
    }

    private void ResetTimer()
    {
        timer.Change(animationInterval, TimeSpan.FromMilliseconds(-1));
    }

    private void TimerHandler(object state)
    {
        lock (timer)
        {
            if (disposed)
            {
                return;
            }

            var progressBlockCount = (int)(currentProgress * blockCount);
            var percent = (int)(currentProgress * 100);
            var text = string.Format("{4}  [{0}{1}] {2,3}% {3}",
                new string('#', progressBlockCount),
                new string('-', blockCount - progressBlockCount),
                percent,
                animation[animationIndex++ % animation.Length],
                BaseMessage);
            UpdateText(text);

            ResetTimer();
        }
    }

    private void UpdateText(string text)
    {
        // Get length of common portion
        var commonPrefixLength = 0;
        var commonLength = Math.Min(currentText.Length, text.Length);
        while (commonPrefixLength < commonLength &&
               text[commonPrefixLength] == currentText[commonPrefixLength])
        {
            commonPrefixLength++;
        }

        // Backtrack to the first differing character
        var outputBuilder = new StringBuilder();
        outputBuilder.Append('\b', currentText.Length - commonPrefixLength);

        // Output new suffix
        outputBuilder.Append(text.Substring(commonPrefixLength));

        // If the new text is shorter than the old one: delete overlapping characters
        var overlapCount = currentText.Length - text.Length;
        if (overlapCount > 0)
        {
            outputBuilder.Append(' ', overlapCount);
            outputBuilder.Append('\b', overlapCount);
        }

        Console.Write(outputBuilder);
        currentText = text;
    }
}
