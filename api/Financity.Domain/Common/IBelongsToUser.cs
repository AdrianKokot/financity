using Financity.Domain.Entities;

namespace Financity.Domain.Common;

public interface IBelongsToUser
{
    public Guid UserId { get; set; }
    public User User { get; set; }
}