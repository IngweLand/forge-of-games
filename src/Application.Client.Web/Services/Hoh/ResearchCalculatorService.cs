using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Factories.Interfaces;
using Ingweland.Fog.Application.Client.Web.Services.Abstractions;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh.Research;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;
using Ingweland.Fog.Dtos.Hoh.Research;
using Ingweland.Fog.Models.Hoh.Entities;
using Ingweland.Fog.Models.Hoh.Enums;
using QuikGraph;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class ResearchCalculatorService(
    IMapper mapper,
    IResearchService researchService,
    IAgeTechnologiesFactory ageTechnologiesFactory,
    ICommonService commonService,
    IPersistenceService persistenceService)
    : IResearchCalculatorService
{
    private CityId _currentCity;
    private BidirectionalGraph<string, Edge<string>> _currentGraph = null!;
    private HashSet<string> _openTechnologies = [];
    private Dictionary<string, int>? _resourcesOrder;
    private readonly HashSet<string> _targetTechnologies = [];
    private readonly Dictionary<CityId, Dictionary<string, TechnologyDto>> _technologies = new();
    private Dictionary<string, ResearchCalculatorTechnologyViewModel> _techViewModels = null!;

    public async Task<IReadOnlyCollection<AgeTechnologiesViewModel>> InitializeAsync(CityId cityId)
    {
        _currentCity = cityId;
        if (!_technologies.TryGetValue(cityId, out var technologies))
        {
            var unprocessedTechnologies = await researchService.GetTechnologiesAsync(cityId);
            var filtered = unprocessedTechnologies.Where(t => t.Age.Index > 1);

            technologies = filtered.ToDictionary(t => t.Id);
            _technologies[cityId] = technologies;
        }

        var ageGroups = technologies.Values.OrderBy(t => t.Age.Index).GroupBy(t => t.Age.Id);
        var viewModel = new List<AgeTechnologiesViewModel>();
        foreach (var g in ageGroups)
        {
            var age = g.First().Age;
            var sorted = g.OrderBy(t => t.HorizontalIndex).ThenBy(t => t.VerticalIndex);
            viewModel.Add(ageTechnologiesFactory.Create(sorted, age));
        }

        CreateGraph(technologies.Values);
        _techViewModels = viewModel.SelectMany(src => src.Technologies).ToDictionary(t => t.Id);
        return viewModel;
    }
    
    public void SelectOpenTechnologies(string selectedTechnologyId)
    {
        var ancestors = GetAncestors(selectedTechnologyId);
        var descendants = GetDescendants(selectedTechnologyId);
        _openTechnologies.Add(selectedTechnologyId);
        _openTechnologies.ExceptWith(descendants);
        _openTechnologies.UnionWith(ancestors);

        Task.Run(async () => await persistenceService.SaveOpenTechnologies(CityId.Capital, _openTechnologies));

        foreach (var openTech in _openTechnologies)
        {
            _techViewModels[openTech].State = ResearchCalculatorTechnologyState.Open;
        }

        foreach (var descendant in descendants)
        {
            _techViewModels[descendant].State = ResearchCalculatorTechnologyState.None;
        }
    }
    
    public void SelectOpenTechnologies(IEnumerable<string> selectedTechnologyIds)
    {
        _openTechnologies = new HashSet<string>(selectedTechnologyIds);
        
        Task.Run(async () => await persistenceService.SaveOpenTechnologies(CityId.Capital, _openTechnologies));

        foreach (var vm in _techViewModels.Values)
        {
            vm.State = ResearchCalculatorTechnologyState.None;
        }
        
        foreach (var openTech in _openTechnologies)
        {
            _techViewModels[openTech].State = ResearchCalculatorTechnologyState.Open;
        }
    }

    public void SelectTargetTechnologies(string selectedTechnologyId)
    {
        if (_openTechnologies.Contains(selectedTechnologyId))
        {
            return;
        }

        var ancestors = GetAncestors(selectedTechnologyId);
        ancestors.ExceptWith(_openTechnologies);
        var descendants = GetDescendants(selectedTechnologyId);
        if (_targetTechnologies.Contains(selectedTechnologyId))
        {
            descendants.Add(selectedTechnologyId);
        }
        else
        {
            _targetTechnologies.Add(selectedTechnologyId);
        }

        _targetTechnologies.ExceptWith(descendants);
        _targetTechnologies.UnionWith(ancestors);

        foreach (var targetTech in _targetTechnologies)
        {
            _techViewModels[targetTech].State = ResearchCalculatorTechnologyState.Target;
        }

        foreach (var descendant in descendants)
        {
            _techViewModels[descendant].State = ResearchCalculatorTechnologyState.None;
        }
    }

    public void ClearTargetTechnologies()
    {
        foreach (var targetTech in _targetTechnologies)
        {
            _techViewModels[targetTech].State = ResearchCalculatorTechnologyState.None;
        }

        _targetTechnologies.Clear();
    }

    public async Task<ResearchCostViewModel> CalculateCost()
    {
        var cityTechnologies = _technologies[_currentCity];
        var targetTechnologies = _targetTechnologies.Select(tech => cityTechnologies[tech]).ToList();
        var cost = targetTechnologies.SelectMany(t => t.Costs).GroupBy(ra => ra.ResourceId)
            .Select(g => new ResourceAmount
            {
                ResourceId = g.Key,
                Amount = g.Sum(ra => ra.Amount),
            });

        await LoadResourcesAsync();
        cost = cost.OrderBy(x => _resourcesOrder!.GetValueOrDefault(x.ResourceId, int.MaxValue)).ToList();

        return new ResearchCostViewModel
        {
            Cost = mapper.Map<IReadOnlyCollection<IconLabelItemViewModel>>(cost),
        };
    }

    private async Task LoadResourcesAsync()
    {
        if (_resourcesOrder != null)
        {
            return;
        }

        _resourcesOrder = (await commonService.GetResourceAsync())
            .OrderBy(x => x.Age?.Index ?? int.MaxValue)
            .ThenBy(x => x.Type)
            .Select((resource, index) => new {resource.Id, index})
            .ToDictionary(x => x.Id, x => x.index);
    }

    private void CreateGraph(ICollection<TechnologyDto> technologies)
    {
        _currentGraph = new BidirectionalGraph<string, Edge<string>>();
        _currentGraph.AddVertexRange(technologies.Select(t => t.Id));
        foreach (var t in technologies)
        {
            var edges = t.ParentTechnologies.Select(parentId => new Edge<string>(parentId, t.Id));
            _currentGraph.AddEdgeRange(edges);
        }
    }

    private HashSet<string> GetAncestors(string id)
    {
        var ancestors = new HashSet<string>();
        var stack = new Stack<string>();
        stack.Push(id);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var edge in _currentGraph.InEdges(current)) // Check in-edges (parents)
            {
                if (ancestors.Add(edge.Source))
                {
                    stack.Push(edge.Source);
                }
            }
        }

        return ancestors;
    }

    private HashSet<string> GetDescendants(string id)
    {
        var descendants = new HashSet<string>();
        var stack = new Stack<string>();
        stack.Push(id);

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            foreach (var edge in _currentGraph.OutEdges(current)) // Check out-edges (children)
            {
                if (descendants.Add(edge.Target))
                {
                    stack.Push(edge.Target);
                }
            }
        }

        return descendants;
    }
}
