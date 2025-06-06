using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Abstractions;
using Ingweland.Fog.Models.Hoh.Entities.Battle;
using Ingweland.Fog.Models.Hoh.Entities.Units;

namespace HohProtoParser.Converters;

public class HeroBattleWaveDefinitionDtoConverter : ITypeConverter<HeroBattleWaveDefinitionDTO, BattleWave>
{
    public BattleWave Convert(HeroBattleWaveDefinitionDTO source, BattleWave destination, ResolutionContext context)
    {
        var units = (IDictionary<string, Unit>) context.Items[ContextKeys.UNITS];
        var squads = new List<BattleWaveSquadBase>();
        foreach (var squadDto in source.Squads)
        {
            BattleWaveSquadBase squad;
            switch (squadDto.SquadCase)
            {
                case HeroBattleWaveSquadDto.SquadOneofCase.Hero:
                {
                    squad = context.Mapper.Map<BattleWaveHeroSquad>(squadDto.Hero);
                    break;
                }
                case HeroBattleWaveSquadDto.SquadOneofCase.Unit:
                {
                    squad = context.Mapper.Map<BattleWaveUnitSquad>((squadDto.Unit, units[squadDto.Unit.Id]));
                    break;
                }
                case HeroBattleWaveSquadDto.SquadOneofCase.None:
                default:
                {
                    throw new Exception($"No squad found on {source.Id}");
                }
            }

            squads.Add(squad);
        }

        return new BattleWave()
        {
            Id = source.Id,
            Squads = squads.AsReadOnly(),
        };
    }
}
