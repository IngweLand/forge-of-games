using System.Text.Json;
using Azure.Storage.Queues;
using Ingweland.Fog.Infrastructure.Repositories.Abstractions;

namespace Ingweland.Fog.Infrastructure.Repositories;

public class QueueRepository<T>(string connectionString, string queueName) : IQueueRepository<T>
{
    private readonly Lazy<QueueClient> _queueClient = new(() => new QueueClient(connectionString, queueName,
        new QueueClientOptions
        {
            MessageEncoding = QueueMessageEncoding.Base64,
        }));

    public async Task SendMessageAsync(T payload)
    {
        var message = JsonSerializer.Serialize(payload);
        await _queueClient.Value.SendMessageAsync(message);
    }
}
