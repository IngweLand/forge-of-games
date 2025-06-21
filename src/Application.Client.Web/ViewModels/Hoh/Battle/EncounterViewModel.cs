using Ingweland.Fog.Models.Hoh.Enums;

namespace Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Battle;

public class EncounterViewModel
{
    public IReadOnlyDictionary<Difficulty, EncounterDetailsViewModel> Details { get; init; } =
        new Dictionary<Difficulty, EncounterDetailsViewModel>();

    public required string Title { get; init; }
}
