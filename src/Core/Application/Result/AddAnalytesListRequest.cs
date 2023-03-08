using MediCare.Application.Medical;
using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AddAnalytesListRequest : IRequest<List<AnalyteDto>>
{
    public List<AddAnalytesListDto> AnalytesList { get; set; }

}

public class AnalyteListFilter : PaginationFilter, IRequest<PaginationResponse<AnalyteDto>>
{

}

public class AddAnalytesListRequestHandler : IRequestHandler<AddAnalytesListRequest, List<AnalyteDto>>
{
    private readonly IAnalyteService _analyteService;

    public AddAnalytesListRequestHandler(IAnalyteService analyteService)
    {
        _analyteService = analyteService;
    }

    public async Task<List<AnalyteDto>> Handle(AddAnalytesListRequest request, CancellationToken cancellationToken)
    {
        return await _analyteService.AddAnalytesListAsync(request, cancellationToken);
    }
}

public class AnalyteListFilterHandler : IRequestHandler<AnalyteListFilter, PaginationResponse<AnalyteDto>>
{
    private readonly IRepository<Analyte> _analyteRepository;

    public AnalyteListFilterHandler(IRepository<Analyte> analyteRepository)
    {
        _analyteRepository = analyteRepository;
    }

    public async Task<PaginationResponse<AnalyteDto>> Handle(AnalyteListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<Analyte, AnalyteDto>(filter);

        return await _analyteRepository.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize);
    }
}