using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Tools;
using Ingweland.Fog.Models.Fog.Entities;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IBuildingMultilevelCostViewModelFactory
{
    BuildingMultilevelCostViewModel Create(BuildingMultilevelCost cost);
}
