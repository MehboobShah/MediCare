using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AnalyteDto : IDto
{
    public DefaultIdType Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}
