using Finbuckle.MultiTenant.EntityFrameworkCore;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Infrastructure.Persistence.Configuration;

public class LabConfig : IEntityTypeConfiguration<Lab>
{
    public void Configure(EntityTypeBuilder<Lab> builder)
    {
        builder
           .ToTable(nameof(Lab), SchemaNames.Medical)
           .HasKey(p => p.Id);

        builder
            .HasMany(p => p.PatientReports)
            .WithOne(pr => pr.Lab)
            .HasForeignKey(pr => pr.LabId)
            .HasPrincipalKey(p => p.Id);

        builder
            .HasMany(p => p.LabTestTypes)
            .WithOne(pr => pr.Lab)
            .HasForeignKey(pr => pr.LabId)
            .HasPrincipalKey(p => p.Id);
    }
}

public class TestTypeConfig : IEntityTypeConfiguration<TestType>
{
    public void Configure(EntityTypeBuilder<TestType> builder)
    {
        builder
           .ToTable(nameof(TestType), SchemaNames.Medical)
           .HasKey(p => p.Id);

        builder
            .HasMany(p => p.PatientReports)
            .WithOne(pr => pr.TestType)
            .HasForeignKey(pr => pr.TestTypeId)
            .HasPrincipalKey(p => p.Id);

        builder
            .HasMany(p => p.LabTestTypes)
            .WithOne(pr => pr.TestType)
            .HasForeignKey(pr => pr.TestTypeId)
            .HasPrincipalKey(p => p.Id);

        builder
            .HasMany(p => p.TestTypeAnalytes)
            .WithOne(pr => pr.TestType)
            .HasForeignKey(pr => pr.TestTypeId)
            .HasPrincipalKey(p => p.Id);
    }
}