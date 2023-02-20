using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class LabTestType : AuditableEntity, IAggregateRoot
{
    public DefaultIdType TestTypeId { get; set; }
    public DefaultIdType LabId { get; set; }
    public Lab Lab { get; set; }
    public TestType TestType { get; set; }

}
