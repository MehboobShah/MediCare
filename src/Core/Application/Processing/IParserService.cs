using MediCare.Application.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Processing;
public interface IParserService : ITransientService
{

    Task<PatientDetailsDto> UploadPdfAsync(UploadPdfRequest request, string userId, CancellationToken cancellationToken);
    Task<bool> UploadPatientDetailsAsync(UpdatePateintDetailsRequest request, string userId, CancellationToken cancellationToken);

}
