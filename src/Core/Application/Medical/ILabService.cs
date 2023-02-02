using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;
public interface ILabService : ITransientService
{
    Task<List<LabDto>> GetAllAsync(CancellationToken cancellationToken);

}
