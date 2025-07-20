using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Dtos.Hoh.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IResearchCalculatorService
{
    Task<IReadOnlyCollection<AgeTechnologiesViewModel>> InitializeAsync(CityId cityId);
    void SelectOpenTechnologies(string selectedTechnologyId);
    void SelectTargetTechnologies(string selectedTechnologyId);
    void ClearTargetTechnologies();
    Task<ResearchCostViewModel> CalculateCost();
    void SelectOpenTechnologies(IEnumerable<string> selectedTechnologyIds);
    Task<IReadOnlyCollection<CityDto>> GetCitiesAsync();
}
