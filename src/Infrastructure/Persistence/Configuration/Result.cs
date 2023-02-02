using Finbuckle.MultiTenant.EntityFrameworkCore;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Infrastructure.Persistence.Configuration;

public class AnalyteConfig : IEntityTypeConfiguration<Analyte>
{
    public void Configure(EntityTypeBuilder<Analyte> builder)
    {
        builder
           .ToTable(nameof(Analyte), SchemaNames.Result)
           .HasKey(p => p.Id);

        builder
            .HasMany(p => p.AnalyteResults)
            .WithOne(pr => pr.Analyte)
            .HasForeignKey(pr => pr.AnalyteId)
            .HasPrincipalKey(p => p.Id);

        builder
            .HasMany(p => p.TestTypeAnalytes)
            .WithOne(pr => pr.Analyte)
            .HasForeignKey(pr => pr.AnalyteId)
            .HasPrincipalKey(p => p.Id);
    }
}