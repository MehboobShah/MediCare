using MediatR;
using MediCare.Application.Medical;
using MediCare.Application.Ontology;
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

    [HttpPost("search")]
    [DisableCors]
    [OpenApiOperation("Gets All Keyword Dictionaries", "")]
    public async Task<ActionResult<PaginationResponse<KeywordDto>>> SearchAsync(KeywordListFilter filter, CancellationToken cancellationToken)
    {
        return Ok(await Mediator.Send(filter, cancellationToken));
    }

    [HttpPut]
    [OpenApiOperation("Updates Keywords", "")]
    public async Task<DefaultIdType> UpdateKeywordsListAsync(UpdateKeywordsRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

}
