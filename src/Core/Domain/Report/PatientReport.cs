using MediCare.Domain.Identity;
using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class PatientReport : AuditableEntity, IAggregateRoot
{
    public string UserId { get; set; }
    public DefaultIdType TestTypeId { get; set; }
    public DefaultIdType LabId { get; set; }
    public TestType TestType { get; set; }
    public Lab Lab { get; set; }
    public ReferentialUser User { get; set; }
    public List<AnalyteResult> AnalyteResults { get; set; }

}
