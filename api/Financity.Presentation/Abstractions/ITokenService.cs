using Financity.Domain.Entities;

namespace Financity.Presentation.Abstractions;

public interface ITokenService
{
    public string GetTokenForUser(User user);
}