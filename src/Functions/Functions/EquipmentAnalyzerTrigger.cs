using System;
using Ingweland.Fog.Functions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Ingweland.Fog.Functions.Functions;

public class EquipmentAnalyzerTrigger(IEquipmentAnalyzer analyzer, ILogger<EquipmentAnalyzerTrigger> logger)
{
    [Function("EquipmentAnalyzerTrigger")]
    public async Task Run([TimerTrigger("0 0 0 1 1 1", RunOnStartup = false)] TimerInfo myTimer)
    {
        await analyzer.Analyze();
    }
}
