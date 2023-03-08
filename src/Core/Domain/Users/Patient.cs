using MediCare.Domain.Identity;
using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Users;
public class Patient : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public ReferentialUser User { get; set; }

}
