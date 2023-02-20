using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;
public class AddTestTypesRequest : IRequest<bool>
{
    public string Name { get; set; }
    public List<DefaultIdType> AnalyteIds { get; set; }
}

public class GetTestTypesRequest : IRequest<List<TestTypeDto>>
{
}

public class AddTestTypesRequestHandler : IRequestHandler<AddTestTypesRequest, bool>
{
    private readonly ITestTypeService _testTypeService;

    public AddTestTypesRequestHandler(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }

    public async Task<bool> Handle(AddTestTypesRequest request, CancellationToken cancellationToken)
    {
        return await _testTypeService.AddTestTypesAsync(request, cancellationToken);
    }
}

public class GetTestTypesRequestHandler : IRequestHandler<GetTestTypesRequest, List<TestTypeDto>>
{
    private readonly ITestTypeService _testTypeService;

    public GetTestTypesRequestHandler(ITestTypeService testTypeService)
    {
        _testTypeService = testTypeService;
    }

    public async Task<List<TestTypeDto>> Handle(GetTestTypesRequest request, CancellationToken cancellationToken)
    {
        return await _testTypeService.GetAllTestTypeAsync(cancellationToken);
    }
}
