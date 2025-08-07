namespace Ingweland.Fog.Infrastructure.Repositories.Abstractions;

public interface IQueueRepository<T>
{
    Task SendMessageAsync(T payload);
}
