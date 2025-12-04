using Ingweland.Fog.Application.Client.Web.ViewModels;

namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IFogCommonUiService
{
    Task<AnnualBudgetViewModel?> GetAnnualBudget();
}
