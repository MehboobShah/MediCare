using MediatR;
using MediCare.Application.Medical;
using MediCare.Application.Processing;
using MediCare.Application.Result;
using Microsoft.AspNetCore.Cors;

namespace MediCare.Host.Controllers.Medical;

public class KeywordController : VersionNeutralApiController
{

    [HttpPost]
    [OpenApiOperation("Adds Keywords", "")]
    public async Task<bool> AddKeywordsListAsync(AddKeywordsRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

}
