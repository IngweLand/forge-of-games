using Ingweland.Fog.Application.Core.Helpers;
using Ingweland.Fog.Dtos.Hoh;
using Refit;

namespace Ingweland.Fog.Application.Core.Services;

public interface IFogCommonService
{
    [Get(FogUrlBuilder.ApiRoutes.ANNUAL_BUDGET_TEMPLATE_REFIT)]
    Task<AnnualBudgetDto?> GetAnnualBudgetAsync(int year);
}
