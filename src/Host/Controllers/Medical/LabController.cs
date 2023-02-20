using MediatR;
using MediCare.Application.Medical;
using MediCare.Application.Processing;
using Microsoft.AspNetCore.Cors;

namespace MediCare.Host.Controllers.Medical;

public class LabController : VersionNeutralApiController
{

    [HttpPost]
    [OpenApiOperation("Adds Labs", "")]
    public async Task<bool> AddLabsAsync(AddLabsRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpPost("search")]
    [DisableCors]
    [OpenApiOperation("Gets All Labs", "")]
    public async Task<ActionResult<PaginationResponse<LabDto>>> SearchAsync(LabListFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(filter, cancellationToken));
    }

}
