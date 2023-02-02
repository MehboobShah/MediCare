using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
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
public class LabService : ILabService
{
    private readonly IRepository<Lab> _labRepository;
    private readonly IStringLocalizer _t;

    public LabService(
        IRepository<Lab> labRepository,
        IStringLocalizer<ParserService> localizer)
    {
        _labRepository = labRepository;
        _t = localizer;

    }

    public async Task<List<LabDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var labs = await _labRepository.GetAll().Select(lr => new LabDto()
        {
            Id = lr.Id,
            Name = lr.Name
        }).ToListAsync();

        return labs;
    }

}
