using Financity.Domain.Common;
using Financity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Financity.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    public ICurrentUserService UserService { get; }
    public DbSet<T> GetDbSet<T>() where T : class;
    public Task<int> DeleteFromSetAsync<T>(Guid id, CancellationToken ct) where T : class, IEntity;
    public Task<int> DeleteFromSetAsync<T>(IQueryable<T> query, CancellationToken ct) where T : class, IEntity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    public IQueryable<Transaction> SearchUserTransactions(Guid userId, string searchTerm, Guid? walletId = null);
    public Task<int> GenerateDefaultCategories(Guid walletId, CancellationToken ct);
}