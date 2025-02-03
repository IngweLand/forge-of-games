using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;

public interface IResearchCalculatorService
{
    Task<IReadOnlyCollection<AgeTechnologiesViewModel>> GetTechnologiesAsync(CityId cityId);
    void SelectOpenTechnologies(string selectedTechnologyId);
    void SelectTargetTechnologies(string selectedTechnologyId);
    void ClearTargetTechnologies();
    ResearchCostViewModel CalculateCost();
}
