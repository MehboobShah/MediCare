using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class TestTypeAnalyte : AuditableEntity, IAggregateRoot
{
    public DefaultIdType AnalyteId { get; set; }
    public DefaultIdType TestTypeId { get; set; }
    public Analyte Analyte { get; set; }
    public TestType TestType { get; set; }

}
