using Finbuckle.MultiTenant.EntityFrameworkCore;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Infrastructure.Persistence.Configuration;

public class PatientConfig : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder
            .ToTable(nameof(Patient), SchemaNames.Users)
            .HasKey(p => p.Id);

        builder
            .HasMany(p => p.PatientReports)
            .WithOne(pr => pr.Patient)
            .HasForeignKey(pr => pr.PatientId)
            .HasPrincipalKey(p => p.Id);
    }
}