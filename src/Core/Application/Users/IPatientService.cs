using MediCare.Domain.Ontology;
using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Users;
public interface IPatientService : ITransientService
{
    Task<bool> AddPatientAsync(Patient patient, CancellationToken cancellationToken);

}
