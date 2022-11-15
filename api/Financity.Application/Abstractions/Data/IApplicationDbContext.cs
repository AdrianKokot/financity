using Financity.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    public DbSet<T> GetDbSet<T>() where T : class;
    public Task<int> DeleteFromSetAsync<T>(Guid id, CancellationToken ct) where T : class, IEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}