namespace Ingweland.Fog.Application.Core.Repository.Abstractions;

public interface IDataProvider
{
    Task InitializeAsync(string version);
}
