using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using MediCare.Application.Processing;
using MediCare.Application.Users;
using MediCare.Host.Controllers;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;

namespace MediCare.Host.Controllers.Processing;

public class ParserController : VersionNeutralApiController
{
    [HttpPost]
    [DisableCors]
    [OpenApiOperation("Uploads Pdf.", "")]
    public async Task<PatientDetailsDto> UploadPdfAsync(UploadPdfRequest request, CancellationToken cancellationToken)
    {
        return await Mediator.Send(request, cancellationToken);
    }

}