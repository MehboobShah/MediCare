using MediCare.Application.Result;
using MediCare.Domain.Ontology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Ontology;
public interface IDictionaryService : ITransientService
{
    Task<List<Keyword>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> AddKeywordsAsync(AddKeywordsRequest request, CancellationToken cancellationToken);
    Task<DefaultIdType> UpdateKeywordsAsync(UpdateKeywordsRequest request, CancellationToken cancellationToken);

}
