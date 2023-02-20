using MediCare.Application.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;
public interface IPatientReportService : ITransientService
{
    Task<DefaultIdType> AddPatientReportAsync(AddPatientReportRequest patientReportRequest, string userId, CancellationToken cancellationToken);

}
