using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class Lab : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public List<LabTestType> LabTestTypes { get; set; }
    public List<PatientReport> PatientReports { get; set; }


}
