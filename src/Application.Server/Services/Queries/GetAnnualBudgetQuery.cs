using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ingweland.Fog.Application.Server.Interfaces;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ingweland.Fog.Application.Server.Services.Queries;

public record GetAnnualBudgetQuery(int Year) : IRequest<AnnualBudgetDto?>, ICacheableRequest
{
    public TimeSpan? Duration => TimeSpan.FromHours(3);
    public DateTimeOffset? Expiration { get; }
}

public class GetAnnualBudgetQueryHandler(IFogDbContext context, IMapper mapper)
    : IRequestHandler<GetAnnualBudgetQuery, AnnualBudgetDto?>
{
    public Task<AnnualBudgetDto?> Handle(GetAnnualBudgetQuery request, CancellationToken cancellationToken)
    {
        return context.AnnualBudgets.Where(x => x.Year == request.Year)
            .ProjectTo<AnnualBudgetDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
}
