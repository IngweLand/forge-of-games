using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Equipment;
using Ingweland.Fog.Dtos.Hoh.Stats;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.Factories.Interfaces;

public interface IEquipmentInsightsViewModelFactory
{
    EquipmentInsightsViewModel Create(EquipmentInsightsDto dto,
        IReadOnlyDictionary<StatAttribute, string> statAttributes, IReadOnlyDictionary<string, string> sets);
}
