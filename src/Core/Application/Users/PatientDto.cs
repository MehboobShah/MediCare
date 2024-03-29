﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Application.Users;
public class PatientDto : IDto
{
    public DefaultIdType Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Gender { get; set; }
    public string? UserId { get; set; }
    public string? Age { get; set; }
    public string? Location { get; set; }
    public string? MedicalRecordNo { get; set; }
    public string? SpecimenNo { get; set; }
    public string? RequestedOn { get; set; }
    public string? ReportedOn { get; set; }
    public string? CollectedOn { get; set; }
    public string? AccountNo { get; set; }
    public string? ReferredBy { get; set; }
    public string? Receipt { get; set; }
    public string? MedicalReportNo { get; set; }
    public string? Ward { get; set; }
    public string? BedNo { get; set; }
}
