using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Ontology;
public class KeywordDto : IDto
{
    public DefaultIdType Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public List<string> Synonyms { get; set; }
}
