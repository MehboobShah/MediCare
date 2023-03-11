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

namespace MediCare.Infrastructure.Patients;
public class PatientService : IPatientService
{
    private readonly IRepository<PatientReport> _patientRepository;

    public PatientService(
        IRepository<PatientReport> patientRepository)
    {
        _patientRepository = patientRepository;

    }

    public async Task<PatientDto> AddPatientAsync(PatientReport patientReport, CancellationToken cancellationToken)
    {
        var id = patientReport.Id;
        await _patientRepository.UpdateAsync(patientReport, cancellationToken);
        await _patientRepository.SaveChangesAsync(cancellationToken);
        var result = await _patientRepository.GetByIdAsync(id, cancellationToken);
        var patientDto = result.Adapt<PatientDto>();

        return patientDto;
    }

}
