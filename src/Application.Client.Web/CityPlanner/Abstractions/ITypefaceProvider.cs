using SkiaSharp;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;

public interface ITypefaceProvider
{
    SKTypeface MainTypeface { get; }
    Task InitializeAsync();
}
