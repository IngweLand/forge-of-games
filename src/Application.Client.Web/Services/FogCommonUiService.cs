using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.Application.Core.Services;

namespace Ingweland.Fog.Application.Client.Web.Services;

public class FogCommonUiService(IFogCommonService fogCommonService) : IFogCommonUiService
{
    public async Task<AnnualBudgetViewModel?> GetAnnualBudget()
    {
        try
        {
            var b = await fogCommonService.GetAnnualBudgetAsync(2026);
            if (b != null)
            {
                return new AnnualBudgetViewModel
                {
                    ServerCostCompletion = b.ServerGoalCompletion,
                    ServerCostCompletionFormatted = b.ServerGoalCompletion.ToString("P1"),
                };
            }
        }
        catch (Exception e)
        {
        }

        return null;
    }
}
