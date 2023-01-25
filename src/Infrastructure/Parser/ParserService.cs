using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Processing;
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

namespace BBD.WebApi.Infrastructure.Exercises;
public class ParserService : IParserService
{
    private readonly IRepository<AnalyteResult> _analyteResultRepository;
    private readonly IRepository<PatientReports> _patientReportsRepository;
    private readonly IRepository<Patient> _patientRepository;

    private readonly IStringLocalizer _t;

    public ParserService(
        IRepository<AnalyteResult> analyteResultRepository,
        IRepository<PatientReports> patientReportsRepository,
        IRepository<Patient> patientRepository,
        IStringLocalizer<ParserService> localizer)
    {
        _analyteResultRepository = analyteResultRepository;
        _patientReportsRepository = patientReportsRepository;
        _patientRepository = patientRepository;
        _t = localizer;

    }

    public async Task<bool> UploadPdfAsync(UploadPdfRequest request, CancellationToken cancellationToken)
    {

        return false;
    }

}
