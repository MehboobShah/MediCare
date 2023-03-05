using MediatR;
using MediCare.Application.Medical;
using MediCare.Application.Processing;
using MediCare.Application.Result;
using Microsoft.AspNetCore.Cors;

namespace MediCare.Host.Controllers.Medical;

public class AnalyteController : VersionNeutralApiController
{

    [HttpPost]
    [OpenApiOperation("Adds Analytes", "")]
    public async Task<bool> AddAnalytesListAsync(AddAnalytesListRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("search")]
    [DisableCors]
    [OpenApiOperation("Gets All Analytes", "")]
    public async Task<ActionResult<PaginationResponse<AnalyteDto>>> SearchAsync(AnalyteListFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(filter, cancellationToken));
    }
}
