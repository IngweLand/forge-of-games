namespace Ingweland.Fog.Functions.Data;

public record ActivityConfiguration(
    int MaxRuns, 
    int? CutOffHour = null, 
    int[]? OnDays = null, 
    int[]? NotOnDays = null);
