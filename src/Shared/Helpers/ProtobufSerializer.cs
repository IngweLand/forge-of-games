using Ingweland.Fog.Shared.Helpers.Interfaces;
using ProtoBuf;

namespace Ingweland.Fog.Shared.Helpers;

public class ProtobufSerializer : IProtobufSerializer
{
    public byte[] SerializeToBytes(object data)
    {
        using var memoryStream = new MemoryStream();
        Serializer.Serialize(memoryStream, data);
        return memoryStream.ToArray();
    }

    public void SerializeToFile(object data, string filePath)
    {
        using var fileStream = File.Create(filePath);
        Serializer.Serialize(fileStream, data);
    }

    public T DeserializeFromBytes<T>(byte[] data)
    {
        using var memoryStream = new MemoryStream(data);
        return Serializer.Deserialize<T>(memoryStream);
    }

    public T DeserializeFromStream<T>(Stream data)
    {
        return Serializer.Deserialize<T>(data);
    }

    public T DeserializeFromFile<T>(string filePath)
    {
        using var fileStream = File.OpenRead(filePath);
        return Serializer.Deserialize<T>(fileStream);
    }
}
