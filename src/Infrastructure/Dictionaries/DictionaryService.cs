using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Catalog.Brands;
using MediCare.Application.Common.Exceptions;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
using MediCare.Application.Ontology;
using MediCare.Application.Processing;
using MediCare.Application.Result;
using MediCare.Domain.Ontology;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediCare.Infrastructure.Dictionaries;
public class DictionaryService : IDictionaryService
{
    private readonly IRepository<Dictionary> _dictionaryRepository;
    private readonly IRepository<Keyword> _keywordRepository;
    private readonly IStringLocalizer<UpdateKeywordsRequestHandler> _localizer;

    public DictionaryService(
        IRepository<Dictionary> dictionaryRepository,
        IRepository<Keyword> keywordRepository,
        IStringLocalizer<UpdateKeywordsRequestHandler> localizer)
    {
        _dictionaryRepository = dictionaryRepository;
        _keywordRepository = keywordRepository;
        _localizer = localizer;
    }

    public async Task<List<Keyword>> GetAllAsync(CancellationToken cancellationToken)
    {
        // var dictionaries = await _dictionaryRepository.GetAll().Select(lr => new Dictionary()).ToListAsync();
        var dictionaries = await _keywordRepository.GetAll().Include(kw => kw.Dictionaries).ToListAsync();

        return dictionaries;
    }

    public async Task<bool> AddKeywordsAsync(AddKeywordsRequest request, CancellationToken cancellationToken)
    {

        var keyword = new Keyword
        {
            Name = request.Keyword,
            Dictionaries = request.Synonyms.Select(d => new Dictionary { Name = d}).ToList()
        };

        await _keywordRepository.AddAsync(keyword, cancellationToken);
        await _keywordRepository.SaveChangesAsync(cancellationToken);

        return true;

    }
    public async Task<DefaultIdType> UpdateKeywordsAsync(UpdateKeywordsRequest request, CancellationToken cancellationToken)
    {
        var keyword = await _keywordRepository.GetAll().Include(k => k.Dictionaries).FirstOrDefaultAsync(k => k.Id == request.Id);
        _ = keyword ?? throw new NotFoundException(_localizer["Keyword Not Found."]);

        keyword.Id = request.Id;
        keyword.Name = request.Name;
        keyword.Dictionaries = request.Synonyms.Select(s => new Dictionary { Name = s}).ToList();

        await _keywordRepository.UpdateAsync(keyword, cancellationToken);
        await _keywordRepository.SaveChangesAsync();
        return keyword.Id;
    }

}
