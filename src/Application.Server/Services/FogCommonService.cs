using Ingweland.Fog.Application.Core.Services;
using Ingweland.Fog.Application.Server.Services.Queries;
using Ingweland.Fog.Dtos.Hoh;
using MediatR;

namespace Ingweland.Fog.Application.Server.Services;

public class FogCommonService(ISender sender):IFogCommonService
{
    public Task<AnnualBudgetDto?> GetAnnualBudgetAsync(int year)
    {
        return sender.Send(new GetAnnualBudgetQuery(year));
    }
}
