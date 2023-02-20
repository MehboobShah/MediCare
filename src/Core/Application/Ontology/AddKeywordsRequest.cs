using MediCare.Application.Medical;
using MediCare.Application.Ontology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AddKeywordsRequest : IRequest<bool>
{
    public string Keyword { get; set; }
    public List<string> Synonyms { get; set; }

}

public class AddKeywordsRequestHandler : IRequestHandler<AddKeywordsRequest, bool>
{
    private readonly IDictionaryService _dictionaryService;

    public AddKeywordsRequestHandler(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    public async Task<bool> Handle(AddKeywordsRequest request, CancellationToken cancellationToken)
    {
        return await _dictionaryService.AddKeywordsAsync(request, cancellationToken);
    }
}