using AutoMapper;
using Ingweland.Fog.HohCoreDataParserSdk.Extensions;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.HohCoreDataParserSdk.Converters;

public class BoostResourceComponentTypeConverter : ITypeConverter<BoostResourceComponentDTO, BoostResourceComponent>
{
    private readonly CityIdListValueConverter _cityIdListValueConverter = new();

    public BoostResourceComponent Convert(BoostResourceComponentDTO source, BoostResourceComponent destination,
        ResolutionContext context)
    {
        var dynamicDefinitions =
            (IList<DynamicFloatValueDefinitionDTO>) context.Items[ContextKeys.DYNAMIC_FLOAT_VALUE_DEFINITIONS];

        var values = new Dictionary<int, double>();
        if (!string.IsNullOrWhiteSpace(source.DynamicValuesId))
        {
            var valueDefinitions = dynamicDefinitions.First(src => src.Id == source.DynamicValuesId);
            if (valueDefinitions.PackedMapping.TypeUrl.EndsWith("BuildingLevelDynamicChangeDTO"))
            {
                var floatValues = context.Mapper.Map<Dictionary<int, float>>(valueDefinitions.PackedMapping
                    .Unpack<BuildingLevelDynamicChangeDTO>());
                values = context.Mapper.Map<Dictionary<int, double>>(floatValues);
            }
        }

        return new BoostResourceComponent()
        {
            Value = source.HasValue ? source.Value : null,
            Values = values,
            CityIds = _cityIdListValueConverter.Convert(source.Cities, context),
            ResourceId = string.IsNullOrWhiteSpace(source.ResourceId) ? null : source.ResourceId,
            ResourceType = string.IsNullOrWhiteSpace(source.ResourceType)
                ? ResourceType.Undefined
                : source.ResourceType.ToResourceType()
        };
    }
}