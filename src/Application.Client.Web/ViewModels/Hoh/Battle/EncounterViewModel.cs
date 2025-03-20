using Ingweland.Fog.Dtos.Hoh.Battle;
using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class EncounterViewModel
{
    public required string Title { get; init; }
    public IReadOnlyDictionary<Difficulty, EncounterDetailsViewModel> Details { get; init; } =
        new Dictionary<Difficulty, EncounterDetailsViewModel>();
}
