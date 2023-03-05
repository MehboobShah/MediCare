using MediCare.Application.Medical;
using MediCare.Application.Ontology;
using MediCare.Domain.Ontology;
using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Ontology;
public class AddKeywordsRequest : IRequest<bool>
{
    public string Keyword { get; set; }
    public List<string> Synonyms { get; set; }

}

public class UpdateKeywordsRequest : IRequest<DefaultIdType>
{
    public DefaultIdType Id { get; set; }
    public string Name { get; set; }
    public List<string> Synonyms { get; set; }

}

public class KeywordListFilter : PaginationFilter, IRequest<PaginationResponse<KeywordDto>>
{

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

public class KeywordListFilterHandler : IRequestHandler<KeywordListFilter, PaginationResponse<KeywordDto>>
{
    private readonly IRepository<Keyword> _keywordRepository;

    public KeywordListFilterHandler(IRepository<Keyword> keywordRepository)
    {
        _keywordRepository = keywordRepository;
    }

    public async Task<PaginationResponse<KeywordDto>> Handle(KeywordListFilter filter, CancellationToken cancellationToken)
    {
        var spec = new EntitiesByPaginationFilterSpec<Keyword, KeywordDto>(filter);

        return await _keywordRepository.PaginatedListAsync(spec, filter.PageNumber, filter.PageSize);
    }
}

public class UpdateKeywordsRequestHandler : IRequestHandler<UpdateKeywordsRequest, DefaultIdType>
{
    private readonly IDictionaryService _dictionaryService;

    public UpdateKeywordsRequestHandler(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    public async Task<DefaultIdType> Handle(UpdateKeywordsRequest request, CancellationToken cancellationToken)
    {
        return await _dictionaryService.UpdateKeywordsAsync(request, cancellationToken);
    }
}