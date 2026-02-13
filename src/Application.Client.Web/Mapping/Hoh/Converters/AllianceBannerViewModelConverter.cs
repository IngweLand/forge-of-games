using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Providers.Interfaces;
using Ingweland.Fog.Application.Client.Web.StatsHub.ViewModels;
using Ingweland.Fog.Models.Hoh.Entities.Alliance;

namespace Ingweland.Fog.Application.Client.Web.Mapping.Hoh.Converters;

public class AllianceBannerViewModelConverter(IAllianceBannerUrlProvider allianceBannerUrlProvider)
    : IValueConverter<AllianceBanner, AllianceBannerViewModel>
{
    public AllianceBannerViewModel Convert(AllianceBanner sourceMember, ResolutionContext context)
    {
        return new AllianceBannerViewModel
        {
            CrestUrl = allianceBannerUrlProvider.GetBackgroundUrl(sourceMember.CrestId, sourceMember.CrestColorId),
            IconUrl = allianceBannerUrlProvider.GetIconUrl(sourceMember.IconId, sourceMember.IconColorId),
        };
    }
}
