using Ardalis.Specification.EntityFrameworkCore;
using BBD.WebApi.Infrastructure.Exercises;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
using MediCare.Application.Processing;
using MediCare.Application.Report;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Parser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediCare.Infrastructure.TestTypes;
public class PatientReportService : IPatientReportService
{
    private readonly IRepository<PatientReport> _patientReportRepository;
    private readonly ILabService _labService;
    private readonly ITestTypeService _testTypeService;



    public PatientReportService(
        ILabService labService,
        ITestTypeService testTypeService,
        IRepository<PatientReport> patientReportRepository)
    {
        _labService = labService;
        _testTypeService = testTypeService;
        _patientReportRepository = patientReportRepository;
    }

    public async Task<PatientReport> AddPatientReportAsync(AddPatientReportRequest patientReportRequest, string userId, CancellationToken cancellationToken)
    {
        var labId = await _labService.GetLabIdAsync(patientReportRequest.LabName, cancellationToken);
        var testTypeId = await _testTypeService.GetTestTypeIdAsync(patientReportRequest.TestType, cancellationToken);
        Debug.WriteLine("hereeeeeeeeeee");
        var patientReport = new PatientReport
        {
            LabId = labId,
            TestTypeId = testTypeId,
            UserId = userId,
            //Name = string.Empty,
            //Gender = string.Empty,
            //Location = string.Empty,
            //Age = string.Empty,
            //AccountNo = string.Empty,
            //AnalyteResults = new List<AnalyteResult>(),
            //BedNo = string.Empty,
            //CollectedOn = string.Empty ,
            //MedicalRecordNo = string.Empty,
            //MedicalReportNo = string.Empty,
            //Receipt = string.Empty,
            //Ward = string.Empty,
            //SpecimenNo= string.Empty,
            //ReferredBy = string.Empty,
            //ReportedOn= string.Empty,
            //RequestedOn = string.Empty
            };

        var result = await _patientReportRepository.AddAsync(patientReport, cancellationToken);

        await _patientReportRepository.SaveChangesAsync(cancellationToken);

        return result;
    }

}
