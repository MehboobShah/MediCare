using MediCare.Application.Result;
using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Users;
public class PatientDetailsDto : IDto
{
    public PatientDto PatientDetails { get; set; }
    public List<AnalyteResultDto> AnalyteResultDetails { get; set; }
}
