using Ardalis.Specification.EntityFrameworkCore;
using Azure.Core;
using BBD.WebApi.Infrastructure.Exercises;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
using MediCare.Application.Processing;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Parser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediCare.Infrastructure.TestTypes;
public class TestTypeService : ITestTypeService
{
    private readonly IRepository<TestType> _testTypeRepository;
    private readonly IRepository<TestTypeAnalyte> _testTypeAnalyteRepository;

    public TestTypeService(
        IRepository<TestType> testTypeRepository,
        IRepository<TestTypeAnalyte> testTypeAnalyteRepository)
    {
        _testTypeRepository = testTypeRepository;
        _testTypeAnalyteRepository = testTypeAnalyteRepository;

    }

    public async Task<List<TestTypeDto>> GetAllTestTypeAsync(CancellationToken cancellationToken)
    {
        var testTypes = await _testTypeRepository.GetAll().Select(lr => new TestTypeDto()
        {
            Id = lr.Id,
            Name = lr.Name.ToLower()
        }).ToListAsync();

        return testTypes;
    }

    public async Task<List<TestTypeAnalyteDto>> GetAllTestTypeAnalyteAsync(string testType, CancellationToken cancellationToken)
    {
        var testTypeId = await _testTypeRepository.GetAll().Where(tt => tt.Name == testType).Select(tt => tt.Id).FirstOrDefaultAsync();

        var analytes = await _testTypeAnalyteRepository.GetAll()
            .Include(tt => tt.Analyte)
            .Where(tt => tt.TestTypeId == testTypeId)
            .Select(tt => new TestTypeAnalyteDto
            {
                Id = tt.AnalyteId,
                Name = tt.Analyte.Name
            }).ToListAsync();

        return analytes;
    }

    public async Task<DefaultIdType> GetTestTypeIdAsync(string testType, CancellationToken cancellationToken)
    {
        return await _testTypeRepository.GetAll().Where(lr => lr.Name == testType.ToLower()).Select(lr => lr.Id).FirstOrDefaultAsync();
    }

    public async Task<bool> AddTestTypesAsync(AddTestTypesRequest request, CancellationToken cancellationToken)
    {
        var testType = new TestType
        {
            Name = request.Name.ToLower(),
            TestTypeAnalytes = request.AnalyteIds.Select(ai => new TestTypeAnalyte
            {
                AnalyteId = ai
            }).ToList()
        };

        var result = await _testTypeRepository.AddAsync(testType, cancellationToken);

        await _testTypeRepository.SaveChangesAsync(cancellationToken);

        return true;

    }

}
