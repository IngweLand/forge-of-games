namespace Ingweland.Fog.Inn.Models.Hoh;

public sealed partial class CityInitDefinitionDTO
{
    public InitialGridComponentDTO InitialGridComponent => PackedComponents
        .Single(any => any.Is(InitialGridComponentDTO.Descriptor)).Unpack<InitialGridComponentDTO>();

}
