using Finbuckle.MultiTenant.EntityFrameworkCore;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Infrastructure.Persistence.Configuration;

public class PatientReportConfig : IEntityTypeConfiguration<PatientReport>
{
    public void Configure(EntityTypeBuilder<PatientReport> builder)
    {
        builder
           .ToTable(nameof(PatientReport), SchemaNames.Report)
           .HasKey(p => p.Id);

        builder
            .HasMany(p => p.AnalyteResults)
            .WithOne(pr => pr.PatientReport)
            .HasForeignKey(pr => pr.PatientReportId)
            .HasPrincipalKey(p => p.Id);

        builder
            .HasOne(ru => ru.User)
            .WithMany(pr => pr.PatientReports)
            .HasForeignKey(pr => pr.UserId)
            .HasPrincipalKey(ru => ru.Id);

    }
}