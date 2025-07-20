using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class StartupDto
{
    public IList<CityDTO> Cities => PackedItems.FindAndUnpackToList<CityDTO>();
    public EquipmentPush? Equipment => PackedItems.FindAndUnpackToList<EquipmentPush>().FirstOrDefault();
    public HeroPush HeroPush => PackedItems.FindAndUnpackToList<HeroPush>().First();

    public IList<InGameEventDto> InGameEvents =>
        PackedItems.FindAndUnpackToList<InGameEventPush>().FirstOrDefault()?.Events ?? [];

    public RelicPush? RelicPush => PackedItems.FindAndUnpackToList<RelicPush>().FirstOrDefault();
    public ResearchStateDTO? ResearchState => PackedItems.FindAndUnpackToList<ResearchStateDTO>().FirstOrDefault();
    public ReworkedWondersDTO? Wonders => PackedItems.FindAndUnpackToList<ReworkedWondersDTO>().FirstOrDefault();
}
