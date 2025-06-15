using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;

namespace HohProtoParser.Converters;

public class CityCultureAreaYValueResolver : IValueResolver<CityCultureAreaComponentDTO, CityCultureAreaComponent, int>
{
    public int Resolve(CityCultureAreaComponentDTO source, CityCultureAreaComponent destination, int destMember,
        ResolutionContext context)
    {
        return -source.Y - source.Height;
    }
}