using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Identity;
public class ReferentialUser : BaseEntity<string>, IAggregateRoot
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public List<PatientReport> PatientReports { get; set; }
}
