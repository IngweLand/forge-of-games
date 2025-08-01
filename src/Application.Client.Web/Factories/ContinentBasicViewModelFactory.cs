using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;
using Ingweland.Fog.Dtos.Hoh.Battle;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class ContinentBasicViewModelFactory : IContinentBasicViewModelFactory
{
    public IReadOnlyCollection<ContinentBasicViewModel> CreateContinents(IEnumerable<ContinentBasicDto> continents)
    {
        var result = new List<ContinentBasicViewModel>();
        var i = 1;
        foreach (var continent in continents)
        {
            var regions = new List<RegionBasicViewModel>();
            foreach (var region in continent.Regions)
            {
                regions.Add(new RegionBasicViewModel
                {
                    Id = region.Id,
                    DisplayIndex = i,
                    Name = $"{i}. {region.Name}",
                });
                i++;
            }

            result.Add(new ContinentBasicViewModel
            {
                Id = continent.Id,
                Name = continent.Name,
                Regions = regions,
            });
        }

        return result.AsReadOnly();
    }
}
