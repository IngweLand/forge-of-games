using AutoMapper;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.City;
using WorkerType = Ingweland.Fog.Models.Hoh.Enums.WorkerType;

namespace HohProtoParser.Converters;

public class GrantWorkerComponentTypeConverter : ITypeConverter<GrantWorkerComponentDTO, GrantWorkerComponent>
{
    public GrantWorkerComponent Convert(GrantWorkerComponentDTO source, GrantWorkerComponent destination,
        ResolutionContext context)
    {
        var dynamicDefinitions =
            (IList<DynamicFloatValueDefinitionDTO>) context.Items[ContextKeys.DYNAMIC_FLOAT_VALUE_DEFINITIONS];

        var values = new Dictionary<int, int>();
        if (!string.IsNullOrWhiteSpace(source.DynamicValuesId))
        {
            var valueDefinitions = dynamicDefinitions.First(src => src.Id == source.DynamicValuesId);
            var floatValues = context.Mapper.Map<Dictionary<int, float>>(valueDefinitions.PackedMapping
                .Unpack<BuildingLevelDynamicChangeDTO>());
            values = context.Mapper.Map<Dictionary<int, int>>(floatValues);
        }

        return new GrantWorkerComponent()
        {
            WorkerCount = source.HasWorkerCount ? source.WorkerCount : null,
            WorkerCounts = values,
            WorkerType = context.Mapper.Map<WorkerType>(source.WorkerType)
        };
    }
}