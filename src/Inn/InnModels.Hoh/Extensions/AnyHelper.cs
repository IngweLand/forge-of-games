using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;

namespace Ingweland.Fog.Inn.Models.Hoh.Extensions;

public static class AnyHelper
{
    public static T FindAndUnpack<T>(this RepeatedField<Any> items) where T : IMessage<T>, new()
    {
        var message = new T();
        var unpacked = items
            .Where(item => Any.GetTypeName(item.TypeUrl) == message.Descriptor.Name)
            .Select(item => item.Unpack<T>())
            .ToList();
        if (unpacked.Count != 1)
        {
            throw new InvalidOperationException($"Expected exactly one instance of {message.Descriptor.Name
            }, but found {unpacked.Count}.");
        }

        return unpacked[0];
    }

    public static IList<T> FindAndUnpackToList<T>(this RepeatedField<Any> items) where T : IMessage<T>, new()
    {
        var message = new T();
        return items
            .Where(item => Any.GetTypeName(item.TypeUrl) == message.Descriptor.Name)
            .Select(item => item.Unpack<T>())
            .ToList();
    }
}
