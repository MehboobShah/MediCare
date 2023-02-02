using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;
public class LabDto : IDto
{
    public DefaultIdType Id { get; set; } = default!;
    public string Name { get; set; } = default!;
}
