﻿using MediCare.Domain.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Medical;
public interface IAnalyteResultService : ITransientService
{
    Task<bool> AddAnalyteResultAsync(AnalyteResult analyteResult, CancellationToken cancellationToken);

}
