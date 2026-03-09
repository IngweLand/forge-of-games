using System.Collections.ObjectModel;

namespace Ingweland.Fog.Application.Core.CityPlanner.Stats;

public static class ResourceConversions
{
    public static readonly IReadOnlyDictionary<string,
            IReadOnlyCollection<(string SourceResourceId, float TargetPerSource)>>
        Rates = new ReadOnlyDictionary<string, IReadOnlyCollection<(string SourceResourceId, float TargetPerSource)>>(
            new Dictionary<string, IReadOnlyCollection<(string SourceResourceId, float TargetPerSource)>>
            {
                ["resource.coffee_beans"] = new List<(string SourceResourceId, float TargetPerSource)>
                {
                    ("resource.gold_fal", 7.72f),
                    ("resource.camel", 166.66f),
                },
                ["resource.brass"] = new List<(string SourceResourceId, float TargetPerSource)>
                {
                    ("resource.gold_fal", 5.75f),
                    ("resource.camel", 125f),
                },
                ["resource.myrrh"] = new List<(string SourceResourceId, float TargetPerSource)>
                {
                    ("resource.gold_fal", 3.32f),
                    ("resource.camel", 73f),
                },
                ["resource.oil"] = new List<(string SourceResourceId, float TargetPerSource)>
                {
                    ("resource.gold_fal", 0.75f),
                    ("resource.camel", 16.5f),
                },
                ["resource.cotton"] = new List<(string SourceResourceId, float TargetPerSource)>
                {
                    ("resource.gold_fal", 1.12f),
                    ("resource.camel", 18.75f),
                },
            });
}
