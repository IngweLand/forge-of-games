using AutoMapper;
using Ingweland.Fog.Application.Client.Web.Services.Hoh.Abstractions;
using Ingweland.Fog.Application.Client.Web.ViewModels.Hoh;
using Ingweland.Fog.Application.Core.Services.Hoh.Abstractions;

namespace Ingweland.Fog.Application.Client.Web.Services.Hoh;

public class CommonUiService : ICommonUiService
{
    private readonly ICommonService _commonService;
    private readonly Lazy<Task<IReadOnlyDictionary<string, AgeViewModel>>> _lazyAges;
    private readonly IMapper _mapper;

    public CommonUiService(IMapper mapper, ICommonService commonService)
    {
        _mapper = mapper;
        _commonService = commonService;
        _lazyAges = new Lazy<Task<IReadOnlyDictionary<string, AgeViewModel>>>(InitializeAges, true);
    }

    public Task<IReadOnlyDictionary<string, AgeViewModel>> GetAgesAsync()
    {
        return _lazyAges.Value;
    }

    public async Task<AgeViewModel?> GetAgeAsync(string ageId)
    {
        var ages = await _lazyAges.Value;
        return ages.GetValueOrDefault(ageId);
    }

    private async Task<IReadOnlyDictionary<string, AgeViewModel>> InitializeAges()
    {
        var ages = await _commonService.GetAgesAsync();
        return _mapper.Map<IEnumerable<AgeViewModel>>(ages.OrderBy(x => x.Index)).ToDictionary(a => a.Id);
    }
}
