using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Server.StatsHub.Factories;

public interface IAllianceWithRankingsFactory
{
    AllianceWithRankings Create(Alliance alliance, IReadOnlyCollection<PlayerDto> currentMembers,
        IReadOnlyCollection<PlayerDto> possiblePastMembers);
}