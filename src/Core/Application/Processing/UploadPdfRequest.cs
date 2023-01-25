
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
    public string PatientId { get; set; } = default!;
    public string TestType { get; set; } = default!;
}

public class UploadPdfRequestHandler : IRequestHandler<UploadPdfRequest, bool>
{
    private readonly IParserService _parserService;

    public UploadPdfRequestHandler(IParserService parserService)
    {
        _parserService = parserService;
    }

    public async Task<bool> Handle(UploadPdfRequest request, CancellationToken cancellationToken)
    {
        return await _parserService.UploadPdfAsync(request, cancellationToken);
    }
}