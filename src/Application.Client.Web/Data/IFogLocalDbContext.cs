using CloudNimble.BlazorEssentials.IndexedDb;

namespace Ingweland.Fog.Application.Client.Web.Data;

public interface IFogLocalDbContext
{
    public IndexedDbObjectStore HohCoreData { get; }
    public IndexedDbObjectStore HohLocalizationData { get; }
}
