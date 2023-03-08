using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Result;
public interface IAnalyteService : ITransientService
{

    Task<List<AnalyteDto>> AddAnalytesListAsync(AddAnalytesListRequest request, CancellationToken cancellationToken);

}
