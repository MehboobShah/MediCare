using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Ontology;
public class Keyword : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    public List<Dictionary> Dictionaries { get; set; }

}
