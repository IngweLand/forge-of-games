namespace Ingweland.Fog.Shared.Helpers.Interfaces;

public interface IProtobufSerializer
{
    byte[] SerializeToBytes(object data);
    void SerializeToFile(object data, string filePath);
    T DeserializeFromBytes<T>(byte[] data);
    T DeserializeFromStream<T>(Stream data);
    T DeserializeFromFile<T>(string filePath);
}
