using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
using MediCare.Application.Ontology;
using MediCare.Application.Processing;
using MediCare.Application.Users;
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

namespace MediCare.Infrastructure.AnalyteResults;
public class AnalyteResultService : IAnalyteResultService
{
    private readonly IRepository<AnalyteResult> _analyteResultRepository;

    public AnalyteResultService(
        IRepository<AnalyteResult> analyteResultRepository)
    {
        _analyteResultRepository = analyteResultRepository;

    }

    public async Task<bool> AddAnalyteResultAsync(AnalyteResult analyteResult, CancellationToken cancellationToken)
    {
        var result = await _analyteResultRepository.AddAsync(analyteResult, cancellationToken);

        await _analyteResultRepository.SaveChangesAsync(cancellationToken);

        return result == null ? false : true;
    }

}
