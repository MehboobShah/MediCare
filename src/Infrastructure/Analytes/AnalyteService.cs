using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
using MediCare.Application.Processing;
using MediCare.Application.Result;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Identity.Client;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBD.WebApi.Infrastructure.Analytes;
public class AnalyteService : IAnalyteService
{
    private readonly IRepository<Analyte> _analyteRepository;
    private readonly IRepository<AnalyteResult> _analyteResultRepository;
    private readonly IRepository<PatientReport> _patientRepository;

    public AnalyteService(
        IRepository<AnalyteResult> analyteResultRepository,
        IRepository<PatientReport> patientRepository,
        IRepository<Analyte> analyteRepository)
    {
        _patientRepository = patientRepository;
        _analyteResultRepository = analyteResultRepository;
        _analyteRepository = analyteRepository;

    }

    public async Task<List<AnalyteDto>> AddAnalytesListAsync(AddAnalytesListRequest request, CancellationToken cancellationToken)
    {
        var analyteDtoList = new List<AnalyteDto>();
        foreach (var item in request.AnalytesList)
        {
            var analyte = new Analyte
            {
                Name = item.Name.ToLower()
            };

            await _analyteRepository.AddAsync(analyte, cancellationToken);
            analyteDtoList.Add(analyte.Adapt<AnalyteDto>());
        }

        await _analyteRepository.SaveChangesAsync(cancellationToken);

        return analyteDtoList;

    }

    public async Task<List<AnalyteResultListDto>> GetAnalyteResultAsync(GetAnalyteResultRequest request, CancellationToken cancellationToken)
    {
        var analyteList = await _patientRepository.GetAll()
            .Include(pr => pr.AnalyteResults.Where(ar => request.AnalyteIds.Contains(ar.AnalyteId)))
            .Where(pr => Convert.ToDateTime(pr.CollectedOn.Substring(0, 10)) >= request.StartDate
            && Convert.ToDateTime(pr.CollectedOn.Substring(0, 10)) <= request.EndDate)
            .Select(pr => pr.AnalyteResults)
            .ToListAsync();

        var analyteDtoList = new List<AnalyteDto>();
        var analyteListDto = analyteList.Adapt<List<AnalyteDto>>();
        foreach (var item in request.AnalytesList)
        {
            var analyte = new Analyte
            {
                Name = item.Name.ToLower()
            };

            await _analyteRepository.AddAsync(analyte, cancellationToken);
            analyteDtoList.Add(analyte.Adapt<AnalyteDto>());
        }

        await _analyteRepository.SaveChangesAsync(cancellationToken);

        return analyteDtoList;

    }

}
