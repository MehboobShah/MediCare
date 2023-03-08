using MediCare.Domain.Ontology;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Users;
public interface IPatientService : ITransientService
{
    Task<PatientDto> AddPatientAsync(PatientReport patientReport, CancellationToken cancellationToken);

}
