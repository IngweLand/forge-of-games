using System.Reflection;
using Ingweland.Fog.Shared.Helpers;
using Ingweland.Fog.Shared.Helpers.Interfaces;
using Refit;

namespace Ingweland.Fog.WebApp.Client.Net;

public class ProtobufHttpContentSerializer(IProtobufSerializer protobufSerializer) : IHttpContentSerializer
{
    private const string ProtobufContentType = "application/x-protobuf";

    public HttpContent ToHttpContent<T>(T item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var content = new ByteArrayContent(protobufSerializer.SerializeToBytes(item));
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ProtobufContentType);

        return content;
    }

    public async Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = new())
    {
        await using var stream = await content.ReadAsStreamAsync(cancellationToken);
        return protobufSerializer.DeserializeFromStream<T>(stream);
    }

    public string? GetFieldNameForProperty(PropertyInfo propertyInfo)
    {
        return propertyInfo.Name;
    }
}
