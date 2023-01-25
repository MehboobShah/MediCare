using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using MediCare.Application.Processing;
using MediCare.Host.Controllers;
using System.Security.Claims;

namespace MediCare.Host.Controllers.Processing;

public class ParserController : VersionNeutralApiController
{
    [HttpPost]
    [OpenApiOperation("Uploads Pdf.", "")]
    public async Task<bool> UploadPdfAsync(UploadPdfRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

}