using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Extensions;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Units;
using Ingweland.Fog.Dtos.Hoh.Units;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class HeroDtoToHeroBasicViewModelConverter(IAssetUrlProvider assetUrlProvider): ITypeConverter<HeroDto, HeroBasicViewModel>
{
    public HeroBasicViewModel Convert(HeroDto source, HeroBasicViewModel destination, ResolutionContext context)
    {
        return new HeroBasicViewModel
        {
            Id = source.Id,
            Name = source.Unit.Name,
            UnitColor = source.Unit.Color.ToCssColor(),
            UnitTypeIconUrl = assetUrlProvider.GetHohIconUrl(source.Unit.Type.GetTypeIconId()),
            PortraitUrl = assetUrlProvider.GetHohUnitPortraitUrl(source.Unit.AssetId),
        };
    }
}
