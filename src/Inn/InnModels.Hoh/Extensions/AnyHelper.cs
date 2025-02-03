using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;

namespace Ingweland.Fog.Inn.Models.Hoh.Extensions;

public static class AnyHelper
{

    public static IList<T> FindAndUnpackToList<T>(this RepeatedField<Any> items) where T : IMessage<T>, new()
    {
        var message = new T();
        return items
            .Where(item => Any.GetTypeName(item.TypeUrl) == message.Descriptor.Name)
            .Select(item => item.Unpack<T>())    
            .ToList();
    }
}
