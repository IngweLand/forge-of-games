using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IResearchCalculatorService
{
    Task<IReadOnlyCollection<AgeTechnologiesViewModel>> InitializeAsync(CityId cityId);
    IReadOnlySet<string> SelectOpenTechnologyWithAncestors(string selectedTechnologyId);
    IReadOnlySet<string> SelectTargetTechnologyWithAncestors(string selectedTechnologyId);
    void ClearTargetTechnologies();
    Task<ResearchCostViewModel> CalculateCost();
    void SelectOpenTechnologies(IEnumerable<string> selectedTechnologyIds);
    Task<IReadOnlyCollection<CityDto>> GetCitiesAsync();
    void SetTargetTechnologiesWithAncestors(IEnumerable<string> selectedTechnologyIds);
    void SetOpenTechnologiesWithAncestors(IEnumerable<string> selectedTechnologyIds);
}
