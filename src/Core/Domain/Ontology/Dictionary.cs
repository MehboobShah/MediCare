using MediCare.Domain.Report;
using MediCare.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Ontology;
public class Dictionary : AuditableEntity, IAggregateRoot
{
    public DefaultIdType KeywordId { get; set; }
    public string Name { get; set; } = default!;
    public Keyword Keyword { get; set; }

}
