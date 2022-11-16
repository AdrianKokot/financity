using System.Security.Claims;
using AutoMapper;
using Financity.Application.Abstractions.Mappings;
using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Auth.Queries;

public sealed record GetUserQuery(ClaimsPrincipal ClaimsPrincipal) : IQuery<UserDetails>;

public sealed class
    GetUserQueryHandler : IQueryHandler<GetUserQuery, UserDetails>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public GetUserQueryHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDetails> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(request.ClaimsPrincipal);

        return _mapper.Map<UserDetails>(user);
    }
}

public sealed record UserDetails(Guid Id, string Email) : IMapFrom<User>;