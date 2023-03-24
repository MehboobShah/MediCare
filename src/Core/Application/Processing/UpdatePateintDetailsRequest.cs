
using MediCare.Application.Result;
using MediCare.Application.Users;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Processing;
public class UpdatePateintDetailsRequest : IRequest<bool>
{
    public PatientDto PatientDetails { get; set; }
    public List<AnalyteResultDto> AnalyteResultDetails { get; set; }
}

public class UpdatePateintDetailsRequestHandler : IRequestHandler<UpdatePateintDetailsRequest, bool>
{
    private readonly IParserService _parserService;
    private readonly ICurrentUser _currentUser;

    public UpdatePateintDetailsRequestHandler(IParserService parserService, ICurrentUser currentUser)
    {
        _parserService = parserService;
        _currentUser = currentUser;
    }

    public async Task<bool> Handle(UpdatePateintDetailsRequest request, CancellationToken cancellationToken)
    {
        string userId = _currentUser.GetUserIdAsString();
        return await _parserService.UploadPatientDetailsAsync(request, userId, cancellationToken);
    }
}