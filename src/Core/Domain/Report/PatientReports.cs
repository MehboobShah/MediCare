using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class PatientReports : AuditableEntity, IAggregateRoot
{
    public DefaultIdType PatientId { get; set; }
    public DefaultIdType TestTypeId { get; set; }
    public DefaultIdType LabId { get; set; }
    public List<Patient> Patients { get; set; }

}
