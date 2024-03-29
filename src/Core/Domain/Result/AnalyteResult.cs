﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Domain.Report;
public class AnalyteResult : AuditableEntity, IAggregateRoot
{
    public DefaultIdType PatientReportId { get; set; }
    public DefaultIdType AnalyteId { get; set; }
    public string? Result { get; set; }
    public string? StartRange { get; set; }
    public string? EndRange { get; set; }
    public Analyte Analyte { get; set; }
    public PatientReport PatientReport { get; set; }
}
