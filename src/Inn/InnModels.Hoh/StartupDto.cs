using Ingweland.Fog.Inn.Models.Hoh.Extensions;

namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class StartupDto
{
    public IList<CityDTO> Cities => PackedItems.FindAndUnpackToList<CityDTO>();
    public HeroPush HeroPush => PackedItems.FindAndUnpackToList<HeroPush>().First();
    public RelicPush? RelicPush => PackedItems.FindAndUnpackToList<RelicPush>().FirstOrDefault();
}
