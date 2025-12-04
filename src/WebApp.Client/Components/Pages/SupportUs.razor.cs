using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels;
using Ingweland.Fog.WebApp.Client.Components.Pages.Abstractions;
using Microsoft.AspNetCore.Components;

namespace Ingweland.Fog.WebApp.Client.Components.Pages;

public partial class SupportUs:FogPageBase
{
    [Inject]
    private IFogCommonUiService FogCommonUiService { get; set; }

    private AnnualBudgetViewModel? _annualBudgetViewModel;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _annualBudgetViewModel = await LoadWithPersistenceAsync(nameof(_annualBudgetViewModel),
            () => FogCommonUiService.GetAnnualBudget());
    }
}
