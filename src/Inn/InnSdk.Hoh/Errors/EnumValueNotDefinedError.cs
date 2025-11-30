using FluentResults;

namespace Ingweland.Fog.InnSdk.Hoh.Errors;

public class EnumValueNotDefinedError<TEnum> : Error where TEnum : Enum
{
    public EnumValueNotDefinedError(object value)
        : base($"Enum value {value} is not defined in enum {typeof(TEnum).FullName}.")
    {
        Value = value;
        WithMetadata("Value", value);
        WithMetadata("Enum type", typeof(TEnum).FullName);
    }

    public object Value { get; }
}
