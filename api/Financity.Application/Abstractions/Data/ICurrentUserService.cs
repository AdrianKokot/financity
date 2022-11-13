namespace Financity.Application.Abstractions.Data;

public interface ICurrentUserService
{
    public bool IsAuthenticated { get; }
    public Guid UserId { get; }
}