﻿using Ardalis.Specification.EntityFrameworkCore;
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
using Microsoft.Identity.Client;
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

    public LabService(
        IRepository<Lab> labRepository)
    {
        _labRepository = labRepository;

    }

    public async Task<List<LabDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var labs = await _labRepository.GetAll().Select(lr => new LabDto()
        {
            Id = lr.Id,
            Name = lr.Name.ToLower()
        }).ToListAsync();

        return labs;
    }

    public async Task<DefaultIdType> GetLabIdAsync(string labName, CancellationToken cancellationToken)
    {
        return await _labRepository.GetAll().Where(lr => lr.Name == labName.ToLower()).Select(lr => lr.Id).FirstOrDefaultAsync();
    }

    public async Task<bool> AddLabsAsync(AddLabsRequest labsRequest, CancellationToken cancellationToken)
    {
        var lab = new Lab
        {
            Name = labsRequest.LabName.ToLower()
        };

        var result = await _labRepository.AddAsync(lab, cancellationToken);
        await _labRepository.SaveChangesAsync(cancellationToken);

        return result != null ? true : false;

    }

}
