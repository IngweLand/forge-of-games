namespace Ingweland.Fog.Models.Hoh.Entities.Dynamic;

public class DynamicFloatValue
{
    private readonly float _value;

    public float Value
    {
        get
        {
            if (Values.Count > 0)
            {
                throw new InvalidOperationException($"You must use {nameof(Values)} list when it's not empty.");
            }

            return _value;
        }
        init => _value = value;
    }

    public IReadOnlyList<float> Values { get; init; } = new List<float>();
}
