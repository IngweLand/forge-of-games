using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace HohProtoParser.Converters;

public class StartupProfile :Profile
{
    public StartupProfile()
    {
        CreateMap<CityMapEntityDto, CityMapEntity>();
        CreateMap<CityDTO, City>()
            .ForMember(dest => dest.CityId, opt => opt.ConvertUsing(new CityIdValueConverter(), src => src.CityId));
    }
}
