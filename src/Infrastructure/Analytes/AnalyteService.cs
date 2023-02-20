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

    public AnalyteService(
        IRepository<Analyte> analyteRepository)
    {
        _analyteRepository = analyteRepository;

    }

    public async Task<bool> AddAnalytesListAsync(AddAnalytesListRequest request, CancellationToken cancellationToken)
    {
        foreach (var analyte in request.AnalytesList)
        {
            var lab = new Analyte
            {
                Name = analyte.Name.ToLower()
            };

            var result = await _analyteRepository.AddAsync(lab, cancellationToken);
        }

        await _analyteRepository.SaveChangesAsync(cancellationToken);

        return true;

    }

}
