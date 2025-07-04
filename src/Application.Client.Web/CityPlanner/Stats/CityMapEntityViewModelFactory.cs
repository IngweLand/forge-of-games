using AutoMapper;
using Ingweland.Fog.Application.Client.Web.CityPlanner.Abstractions;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Localization;
using Ingweland.Fog.Application.Client.Web.Models;
using Ingweland.Fog.Application.Client.Web.Providers;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.City;
using Ingweland.Fog.Application.Core.CityPlanner.Stats;
using Ingweland.Fog.Application.Core.CityPlanner.Stats.BuildingTypedStats;
using Ingweland.Fog.Dtos.Hoh;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Fog.Entities;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Entities.Rewards;
using Ingweland.Fog.Models.Hoh.Enums;
using Microsoft.Extensions.Localization;
using CityMapEntity = Ingweland.Fog.Application.Core.CityPlanner.CityMapEntity;

namespace Ingweland.Fog.Application.Client.Web.CityPlanner.Stats;

public class CityMapEntityViewModelFactory(
    IMapper mapper,
    IAssetUrlProvider assetUrlProvider,
    IHohStorageIconUrlProvider storageIconUrlProvider,
    IHohResourceIconUrlProvider resourceIconUrlProvider,
    IStringLocalizer<FogResource> localizer,
    IWorkerIconUrlProvider workerIconUrlProvider) : ICityMapEntityViewModelFactory
{
    public CityMapEntityViewModel Create(CityMapEntity entity, BuildingDto building,
        BuildingLevelRange levelRange, IReadOnlyCollection<BuildingCustomizationDto> customizations)
    {
        var constructionComponentDto = building.Components.OfType<ConstructionComponent>().FirstOrDefault();
        ConstructionComponentViewModel? constructionComponent = null;
        if (constructionComponentDto != null)
        {
            constructionComponent = mapper.Map<ConstructionComponentViewModel>(constructionComponentDto);
        }

        var upgradeComponentDto = building.Components.OfType<UpgradeComponent>().FirstOrDefault();
        UpgradeComponentViewModel? upgradeComponent = null;
        if (upgradeComponentDto != null)
        {
            upgradeComponent = mapper.Map<UpgradeComponentViewModel>(upgradeComponentDto);
        }

        var infoItems = new List<IconLabelItemViewModel>();
        var happinessProvider = entity.FirstOrDefaultStat<HappinessProvider>();
        if (happinessProvider != null)
        {
            infoItems.Add(new IconLabelItemViewModel()
            {
                Label = happinessProvider.Range.ToString(),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_boost_radius"),
            });
            infoItems.Add(new IconLabelItemViewModel()
            {
                Label = happinessProvider.Value.ToString(),
                IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
            });
        }

        var grantWorkerComponent = building.Components.OfType<GrantWorkerComponent>().FirstOrDefault();
        if (grantWorkerComponent != null)
        {
            infoItems.Add(new IconLabelItemViewModel()
            {
                Label = grantWorkerComponent.GetWorkerCount().ToString(),
                IconUrl = workerIconUrlProvider.GetIcon(building.CityIds.First(), grantWorkerComponent.WorkerType),
            });
        }

        var buildingSizeString = $"{building.Width}x{building.Length}";
        infoItems.Add(new IconLabelItemViewModel()
        {
            Label = buildingSizeString,
            IconUrl = assetUrlProvider.GetHohIconUrl("icon_terrain_land"),
        });

        ProductionComponentViewModel? productionComponentViewModel = null;
        if (building.Type != BuildingType.Evolving)
        {
            var generalItems = new List<IconLabelItemViewModel>();
            var happinessConsumer = entity.FirstOrDefaultStat<HappinessConsumer>();
            if (happinessConsumer != null)
            {
                generalItems.Add(new IconLabelItemViewModel()
                {
                    Label = $"{happinessConsumer.ConsumedHappiness:N0}/{happinessConsumer.BuffDetails.Value:N0}",
                    IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
                });
            }

            var products = new List<CityMapEntityProductViewModel>();
            var productionProvider = entity.FirstOrDefaultStat<ProductionProvider>();
            if (productionProvider != null)
            {
                foreach (var productionStatsItem in productionProvider.ProductionStatsItems)
                {
                    foreach (var product in productionStatsItem.Products)
                    {
                        var canSelectProduct = ProductionProviderHelper.CanSelectProduct(building.Type, building.Group);
                        if (!canSelectProduct || (canSelectProduct &&
                                entity.SelectedProductId == productionStatsItem.ProductionId))
                        {
                            if (productionStatsItem.WorkerCount > 0)
                            {
                                generalItems.Add(new IconLabelItemViewModel()
                                {
                                    Label = productionStatsItem.WorkerCount.ToString(),
                                    IconUrl = workerIconUrlProvider.GetIcon(building.CityIds.First()),
                                });
                            }

                            var defaultHours = product.DefaultProduction.ProductionTime / 3600;
                            generalItems.Add(new IconLabelItemViewModel()
                            {
                                Label = $"{defaultHours}{localizer[FogResource.Common_Hours_Abbr]} - {
                                    product.DefaultProduction.BuffedValue:N0}",
                                IconUrl = building.Type == BuildingType.CityHall
                                    ? storageIconUrlProvider.GetIconUrl(product.ResourceId)
                                    : storageIconUrlProvider.GetIconUrl(building.Type),
                            });
                        }

                        products.Add(new CityMapEntityProductViewModel()
                        {
                            ProductId = productionStatsItem.ProductionId,
                            IconUrl = resourceIconUrlProvider.GetIconUrl(product.ResourceId),
                            OneHourProduction = product.OneHourProduction.BuffedValue.ToString("N0"),
                            OneDayProduction = product.OneDayProduction.BuffedValue.ToString("N0"),
                            IsSelected = productionStatsItem.ProductionId == entity.SelectedProductId,
                        });
                    }
                }
            }

            if (building.Type == BuildingType.Barracks)
            {
                var abilityTrainingComponent =
                    building.Components.OfType<HeroAbilityTrainingComponent>().FirstOrDefault();
                if (abilityTrainingComponent != null)
                {
                    var productionAmount = abilityTrainingComponent.Value;
                    if (happinessConsumer != null)
                    {
                        var factor = MathF.Min(1,
                                (float) happinessConsumer.ConsumedHappiness / happinessConsumer.BuffDetails.Value) *
                            happinessConsumer.BuffDetails.Factor;
                        var oneHourBonus = (int) Math.Floor(happinessConsumer.BuffDetails.Value * factor);
                        productionAmount += oneHourBonus;
                    }

                    products.Add(new CityMapEntityProductViewModel()
                    {
                        ProductId = abilityTrainingComponent.Id,
                        IconUrl = resourceIconUrlProvider.GetIconUrl("resource.mastery_points"),
                        OneHourProduction = ((int) MathF.Floor(productionAmount))
                            .ToString("N0"),
                        OneDayProduction =
                            ((int) MathF.Floor(productionAmount * 24)).ToString(
                                "N0"),
                    });
                }
            }

            if (generalItems.Count > 0 || products.Count > 0)
            {
                productionComponentViewModel = new ProductionComponentViewModel()
                {
                    General = generalItems,
                    Products = products,
                    CanSelectProduct = ProductionProviderHelper.CanSelectProduct(building.Type, building.Group),
                };
            }
        }

        CustomizationComponentViewModel? customizationComponentViewModel = null;
        if (customizations.Count > 0)
        {
            var selectedItem = customizations.FirstOrDefault(bc => bc.Id == entity.CustomizationId);
            selectedItem ??= BuildingCustomizationDto.None;

            var generalItems = new List<IconLabelItemViewModel>();

            foreach (var component in selectedItem.Components)
            {
                if (component is CultureBoostComponent cultureBoostComponent)
                {
                    generalItems.Add(new IconLabelItemViewModel()
                    {
                        Label = $"{cultureBoostComponent.Factor * 100}%",
                        IconUrl = assetUrlProvider.GetHohIconUrl("icon_flat_culture_boost"),
                    });
                }
                else if (component is BoostResourceComponent boostResourceComponent)
                {
                    generalItems.Add(new IconLabelItemViewModel()
                    {
                        Label = $"{boostResourceComponent.Value * 100}%",
                        IconUrl = boostResourceComponent.ResourceId == null
                            ? resourceIconUrlProvider.GetIconUrl(boostResourceComponent.ResourceType)
                            : resourceIconUrlProvider.GetIconUrl(boostResourceComponent.ResourceId),
                    });
                }
                else if (component is ProductionComponent productionComponent)
                {
                    var hours = productionComponent.ProductionTime / 3600;
                    generalItems.AddRange(productionComponent.Products.OfType<ResourceReward>()
                        .Select(resourceProduct => new IconLabelItemViewModel()
                        {
                            Label = $"{hours}{localizer[FogResource.Common_Hours_Abbr]} - {resourceProduct.Amount:N0}",
                            IconUrl = resourceIconUrlProvider.GetIconUrl(resourceProduct.ResourceId),
                        }));
                }
            }

            customizationComponentViewModel = new CustomizationComponentViewModel()
            {
                SelectedItem = selectedItem,
                Items = customizations.Prepend(BuildingCustomizationDto.None).ToList(),
                General = generalItems,
            };
        }

        return new CityMapEntityViewModel()
        {
            Id = entity.Id,
            Name = building.Name,
            Age = mapper.Map<AgeViewModel?>(building.Age),
            Size = buildingSizeString,
            Level = entity.Level,
            InfoItems = infoItems,
            LevelRange = levelRange,
            ProductionComponent = productionComponentViewModel,
            CustomizationComponent = customizationComponentViewModel,
        };
    }
}
