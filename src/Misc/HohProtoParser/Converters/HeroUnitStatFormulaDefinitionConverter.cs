using System.Collections.ObjectModel;
using AutoMapper;
using HohProtoParser.Helpers;
using Ingweland.Fog.Inn.Models.Hoh;
using Ingweland.Fog.Models.Hoh.Entities.Units;
using Ingweland.Fog.Models.Hoh.Enums;

namespace HohProtoParser.Converters;

public class HeroUnitStatFormulaDefinitionConverter:ITypeConverter<HeroUnitStatFormulaDefinitionDTO, UnitStatFormulaData>
{
    public UnitStatFormulaData Convert(HeroUnitStatFormulaDefinitionDTO source, UnitStatFormulaData destination,
        ResolutionContext context)
    {
        var rarityFactors = new Dictionary<string, UnitStatFormulaFactors>();
        //rarityFactors = source.RarityUnits.ToDictionary(dto => dto.RarityId, dto => dto.Factors);
        return new UnitStatFormulaData()
        {
            Type = StringParser.ParseEnumFromString<UnitStatFormulaType>(source.Id),
            BaseFactor = source.Unit.Normal,
            RarityFactors =
                new ReadOnlyDictionary<string, UnitStatFormulaFactors>(
                    new Dictionary<string, UnitStatFormulaFactors>()),
        };
    }
}