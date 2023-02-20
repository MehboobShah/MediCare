using MediCare.Application.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public class AddAnalytesListRequest : IRequest<bool>
{
    public List<AddAnalytesListDto> AnalytesList { get; set; }

}

public class AddAnalytesListRequestHandler : IRequestHandler<AddAnalytesListRequest, bool>
{
    private readonly IAnalyteService _analyteService;

    public AddAnalytesListRequestHandler(IAnalyteService analyteService)
    {
        _analyteService = analyteService;
    }

    public async Task<bool> Handle(AddAnalytesListRequest request, CancellationToken cancellationToken)
    {
        return await _analyteService.AddAnalytesListAsync(request, cancellationToken);
    }
}