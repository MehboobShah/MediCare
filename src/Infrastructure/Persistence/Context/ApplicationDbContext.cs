using Finbuckle.MultiTenant;
using MediCare.Application.Common.Events;
using MediCare.Application.Common.Interfaces;
using MediCare.Domain.Catalog;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using MediCare.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MediCare.Infrastructure.Persistence.Context;

public class ApplicationDbContext : BaseDbContext
{
    public ApplicationDbContext(ITenantInfo currentTenant, DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer, IOptions<DatabaseSettings> dbSettings, IEventPublisher events)
        : base(currentTenant, options, currentUser, serializer, dbSettings, events)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    // Medical
    public DbSet<Lab> Labs => Set<Lab>();
    public DbSet<LabTestType> LabTestTypes => Set<LabTestType>();
    public DbSet<TestType> TestTypes => Set<TestType>();
    public DbSet<TestTypeAnalyte> TestTypeAnalytes => Set<TestTypeAnalyte>();

    // Report
    public DbSet<PatientReport> PatientReports => Set<PatientReport>();

    // Result
    public DbSet<Analyte> Analytes => Set<Analyte>();
    public DbSet<AnalyteResult> AnalyteResults => Set<AnalyteResult>();

    // Users
    public DbSet<Patient> Patients => Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(SchemaNames.Catalog);
    }
}