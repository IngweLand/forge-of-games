using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class AllianceAthRankingViewModelFactory : IAllianceAthRankingViewModelFactory
{
    public AllianceAthRankingViewModel Create(AllianceAthRankingDto dto)
    {
        return new AllianceAthRankingViewModel
        {
            EventLabel = $"{dto.StartedAt:d} - {dto.EndedAt:d}",
            PointsFormatted = dto.Points.ToString("N0"),
        };
    }
}
