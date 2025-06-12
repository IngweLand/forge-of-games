using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;

namespace Ingweland.Fog.Application.Client.Web.Factories;

public class BuildingViewModelFactory(
    IMapper mapper,
    IAssetUrlProvider assetUrlProvider,
    IHohStorageIconUrlProvider storageIconUrlProvider,
    IStringLocalizer<FogResource> localizer) : IBuildingViewModelFactory
{
    public BuildingViewModel Create(BuildingDto source)
    {
        var constructionComponentDto = source.Components.OfType<ConstructionComponent>().FirstOrDefault();
        ConstructionComponentViewModel? constructionComponent = null;
        if (constructionComponentDto != null)
        {
            constructionComponent = mapper.Map<ConstructionComponentViewModel>(constructionComponentDto);
        }

        var upgradeComponentDto = source.Components.OfType<UpgradeComponent>().FirstOrDefault();
        UpgradeComponentViewModel? upgradeComponent = null;
        if (upgradeComponentDto != null)
        {
            upgradeComponent = mapper.Map<UpgradeComponentViewModel>(upgradeComponentDto);
        }
        var buildingSizeString = $"{source.Width}x{source.Length}";

        return new BuildingViewModel()
        {
            Id = source.Id,
            Name = source.Name,
            AgeName = source.CityIds.Contains(CityId.Capital) ? source.Age?.Name : null,
            AgeColor = source.Age.ToCssColor(),
            Size = buildingSizeString,
            Level = source.Level,
            ConstructionComponent = constructionComponent,
            UpgradeComponent = upgradeComponent,
            Data = source,
        };
    }
}
