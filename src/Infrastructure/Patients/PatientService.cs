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
    private readonly IRepository<Patient> _patientRepository;

    public PatientService(
        IRepository<Patient> patientRepository)
    {
        _patientRepository = patientRepository;

    }

    public async Task<bool> AddPatientAsync(Patient patient, CancellationToken cancellationToken)
    {
        var result = await _patientRepository.AddAsync(patient, cancellationToken);

        await _patientRepository.SaveChangesAsync(cancellationToken);

        return result == null ? false : true;
    }

}
