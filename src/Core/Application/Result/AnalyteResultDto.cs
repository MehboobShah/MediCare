using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AnalyteResultDto : IDto
{
    public DefaultIdType Id { get; set; } = default!;
    public DefaultIdType PatientReportId { get; set; }
    public DefaultIdType AnalyteId { get; set; }
    public string? Result { get; set; }
    public string? StartRange { get; set; }
    public string? EndRange { get; set; }
}
