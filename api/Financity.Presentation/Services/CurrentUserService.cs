using System.Security.Claims;
using Financity.Application.Abstractions.Data;
using Financity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Financity.Presentation.Services;

public class CurrentUserService : ICurrentUserService
{

    public CurrentUserService(IHttpContextAccessor httpContext)
    {
        var userId = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId)) return;

        IsAuthenticated = true;
        UserId = Guid.Parse(userId);
    }

    public bool IsAuthenticated { get; }
    public Guid UserId { get; } = Guid.Empty;
}