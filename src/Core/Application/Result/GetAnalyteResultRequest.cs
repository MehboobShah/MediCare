using MediCare.Application.Medical;
using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class GetAnalyteResultRequest : IRequest<List<AnalyteResultListDto>>
{
    public List<DefaultIdType> AnalyteIds { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

}

public class GetAnalyteResultRequestHandler : IRequestHandler<GetAnalyteResultRequest, List<AnalyteResultListDto>>
{
    private readonly IAnalyteService _analyteService;

    public GetAnalyteResultRequestHandler(IAnalyteService analyteService)
    {
        _analyteService = analyteService;
    }

    public async Task<List<AnalyteResultListDto>> Handle(GetAnalyteResultRequest request, CancellationToken cancellationToken)
    {
        return await _analyteService.GetAnalyteResultAsync(request, cancellationToken);
    }
}