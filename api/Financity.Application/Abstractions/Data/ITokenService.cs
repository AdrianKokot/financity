using Financity.Domain.Entities;

namespace Financity.Application.Abstractions.Data;

public interface ITokenService
{
    public string GetTokenForUser(User user);
}