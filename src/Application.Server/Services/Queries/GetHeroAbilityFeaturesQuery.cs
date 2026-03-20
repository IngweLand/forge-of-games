using System.Globalization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh.Units;
using Ingweland.Fog.Shared.Localization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetHeroAbilityFeaturesQuery : IRequest<IReadOnlyCollection<HeroAbilityFeaturesDto>>, ICacheableRequest
{
    public TimeSpan? Duration { get; }
    public DateTimeOffset? Expiration => DateTimeOffset.MaxValue;
}

public class GetHeroAbilityFeaturesQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetHeroAbilityFeaturesQuery, IReadOnlyCollection<HeroAbilityFeaturesDto>>
{
    public async Task<IReadOnlyCollection<HeroAbilityFeaturesDto>> Handle(GetHeroAbilityFeaturesQuery request,
        CancellationToken cancellationToken)
    {
        var cultureCode = HohSupportedCultures.AllCultures.FirstOrDefault(x => x == CultureInfo.CurrentCulture.Name) ??
            HohSupportedCultures.DefaultCulture;
        return await context.HeroAbilityFeatures.Where(x => x.Locale == cultureCode)
            .ProjectTo<HeroAbilityFeaturesDto>(mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
}
