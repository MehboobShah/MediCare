using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Processing;
public interface IParserService : ITransientService
{

    Task<bool> UploadPdfAsync(UploadPdfRequest request, CancellationToken cancellationToken);

}
