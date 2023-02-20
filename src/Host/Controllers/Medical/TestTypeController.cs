using MediCare.Application.Medical;
using MediCare.Domain.Report;

namespace MediCare.Host.Controllers.Medical;

public class TestTypeController : VersionNeutralApiController
{
    [HttpPost]
    [OpenApiOperation("Adds Test Types", "")]
    public async Task<bool> AddTestTypesAsync(AddTestTypesRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

    [HttpGet]
    [OpenApiOperation("Gets All Test Types", "")]
    public async Task<List<TestTypeDto>> GetTestTypesAsync(CancellationToken cancellationToken)
    {
        return await Mediator.Send(new GetTestTypesRequest(), cancellationToken);
    }
}
