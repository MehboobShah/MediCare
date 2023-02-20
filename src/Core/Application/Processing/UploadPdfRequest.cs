
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Processing;
public class UploadPdfRequest : IRequest<bool>
{
    public string ReportText { get; set; } = default!;
    public string LabName { get; set; } = default!;
    public string TestType { get; set; } = default!;
}

public class UploadPdfRequestHandler : IRequestHandler<UploadPdfRequest, bool>
{
    private readonly IParserService _parserService;
    private readonly ICurrentUser _currentUser;

    public UploadPdfRequestHandler(IParserService parserService, ICurrentUser currentUser)
    {
        _parserService = parserService;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(UploadPdfRequest request, CancellationToken cancellationToken)
    {
        string userId = _currentUser.GetUserIdAsString();
        return await _parserService.UploadPdfAsync(request, userId, cancellationToken);
    }
}