using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AnalyteResultListDto : IDto
{
    public DefaultIdType AnalyteId { get; set; }
    public List<AnalyteResultDetailDto> AnalyteResultDetail { get; set; }
}
