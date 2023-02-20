using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class Analyte : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public List<AnalyteResult> AnalyteResults { get; set; }
    public List<TestTypeAnalyte> TestTypeAnalytes { get; set; }

}
