namespace Ingweland.Fog.Application.Client.Web.ViewModels;

public class LabeledValue<T>
{
    public required string Label { get; init; }
    public required T Value { get; init; }
}
