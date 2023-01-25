using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class Analyte : AuditableEntity, IAggregateRoot
{
    public int PatientId { get; set; }
    public int TestTypeId { get; set; }
    public int LabId { get; set; }
}
