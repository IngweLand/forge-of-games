using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BattleEventBasicViewModelFactory : IBattleEventBasicViewModelFactory
{
    public BattleEventBasicViewModel Create(BattleEventBasicDto dto)
    {
        return new BattleEventBasicViewModel
        {
            Id = dto.Id,
            Name = dto.Name,
            Encounters = Enumerable.Range(dto.EncounterStartIndex, dto.EncounterCount).ToList(),
        };
    }
}
