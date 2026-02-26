using Ingweland.Fog.Application.Client.Web.StatsHub.Abstractions;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Dtos.Hoh.Stats;

namespace Ingweland.Fog.Application.Client.Web.StatsHub.Factories;

public class PlayerAthRankingViewModelFactory : IPlayerAthRankingViewModelFactory
{
    public PlayerAthRankingViewModel Create(PlayerAthRankingDto dto)
    {
        return new PlayerAthRankingViewModel
        {
            EventLabel = $"{dto.StartedAt:d} - {dto.EndedAt:d}",
            PointsFormatted = dto.Points.ToString("N0"),
            UpdatedAtFormatted = dto.UpdatedAt.ToString("d"),
        };
    }
}
