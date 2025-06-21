using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class HeroBasicDtoToViewModelConverter : ITypeConverter<HeroBasicDto, HeroBasicViewModel>
{
    private readonly IAssetUrlProvider _assetUrlProvider;

    public HeroBasicDtoToViewModelConverter(IAssetUrlProvider assetUrlProvider)
    {
        _assetUrlProvider = assetUrlProvider;
    }

    public HeroBasicViewModel Convert(HeroBasicDto source, HeroBasicViewModel destination, ResolutionContext context)
    {
        return new HeroBasicViewModel
        {
            Id = source.Id,
            UnitId = source.UnitId,
            Name = source.Name,
            UnitColor = source.UnitColor.ToCssColor(),
            UnitTypeIconUrl = _assetUrlProvider.GetHohIconUrl(source.UnitType.GetTypeIconId()),
            PortraitUrl = _assetUrlProvider.GetHohUnitPortraitUrl(source.AssetId),
        };
    }
}
