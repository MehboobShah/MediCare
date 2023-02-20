using Finbuckle.MultiTenant.EntityFrameworkCore;
using MediCare.Domain.Ontology;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediCare.Infrastructure.Persistence.Configuration;

public class KeywordConfig : IEntityTypeConfiguration<Keyword>
{
    public void Configure(EntityTypeBuilder<Keyword> builder)
    {
        builder
           .ToTable(nameof(Keyword), SchemaNames.Ontology)
           .HasKey(p => p.Id);

        builder
            .HasMany(p => p.Dictionaries)
            .WithOne(pr => pr.Keyword)
            .HasForeignKey(pr => pr.KeywordId)
            .HasPrincipalKey(p => p.Id);

    }
}

