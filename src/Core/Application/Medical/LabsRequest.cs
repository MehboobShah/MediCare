using MediCare.Application.Processing;
using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;

public class AddLabsRequest : IRequest<bool>
{
   public string LabName { get; set; } = default!;

}

public class LabListFilter : PaginationFilter, IRequest<PaginationResponse<LabDto>>
{

}

public class AddLabsRequestHandler : IRequestHandler<AddLabsRequest, bool>
{
    private readonly ILabService _labService;

    public AddLabsRequestHandler(ILabService labService)
    {
        _labService = labService;

    }

    public async Task<bool> Handle(AddLabsRequest request, CancellationToken cancellationToken)
    {
        return await _labService.AddLabsAsync(request, cancellationToken);
    }
}

public class LabListFilterHandler : IRequestHandler<LabListFilter, PaginationResponse<LabDto>>
{
    private readonly ILabService _labService;
    private readonly IRepository<Lab> _labRepository;

    public LabListFilterHandler(ILabService labService, IRepository<Lab> labRepository)
    {
        _labService = labService;
        _labRepository = labRepository;
    }

    public async Task<PaginationResponse<LabDto>> Handle(LabListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<Lab, LabDto>(filter);

        return await _labRepository.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize);
    }
}