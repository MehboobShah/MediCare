using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AnalyteResultDetailDto : IDto
{
    public AnalyteResultDto AnalyteResult { get; set; }
    public DateTime ReportDate { get; set; }
}
