using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using MediCare.Application.Common.Persistence;
using MediCare.Domain.Common.Contracts;
using MediCare.Infrastructure.Persistence.Context;
using Mapster;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace MediCare.Infrastructure.Persistence.Repository;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class ApplicationDbRepository<T> : RepositoryBase<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public readonly ApplicationDbContext Context;
    public readonly DbSet<T> Entities;

    public ApplicationDbRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
        this.Context = dbContext;
        this.Entities = dbContext.Set<T>();
    }

    public IQueryable<T> GetAll()
    {
        return Entities;
    }



    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        ApplySpecification(specification, false)
            .ProjectToType<TResult>();
}