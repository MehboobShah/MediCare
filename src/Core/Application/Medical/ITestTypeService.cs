using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;
public interface ITestTypeService : ITransientService
{
    Task<List<TestTypeDto>> GetAllTestTypeAsync(CancellationToken cancellationToken);
    Task<List<TestTypeAnalyteDto>> GetAllTestTypeAnalyteAsync(string testType, CancellationToken cancellationToken);
    Task<DefaultIdType> GetTestTypeIdAsync(string testType, CancellationToken cancellationToken);
    Task<bool> AddTestTypesAsync(AddTestTypesRequest testTypeRequest, CancellationToken cancellationToken);

}
