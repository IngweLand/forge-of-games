using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities.Equipment;
using Ingweland.Fog.Models.Hoh.Entities.Relics;

namespace Ingweland.Fog.Application.Client.Web.EquipmentConfigurator.Abstractions;

public interface IEquipmentProfilePersistenceService
{
    Task UpsertProfileAsync(string? profileId, string? profileName, IReadOnlyCollection<HeroProfileIdentifier> heroes,
        IReadOnlyCollection<RelicItem> relics, IReadOnlyCollection<EquipmentItem> equipment,
        BarracksProfile barracksProfile);

    Task SaveAsync(EquipmentProfile profile);

    Task<EquipmentProfile?> GetAsync(string profileId);
    ValueTask DeleteAsync(string profileId);
    ValueTask<IReadOnlyCollection<EquipmentProfileBasicData>> GetProfiles();
}
