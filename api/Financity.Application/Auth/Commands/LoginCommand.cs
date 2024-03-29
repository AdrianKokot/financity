﻿using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Auth.Commands;

public sealed record LoginCommand(string Email, string Password) : ICommand<LoginCommandResult>;

public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, LoginCommandResult>
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<LoginCommandResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            throw ValidationExceptionFactory.For(nameof(request.Password), "User with given credentials doesn't exist");

        var token = _tokenService.GetTokenForUser(user);

        return new LoginCommandResult(token);
    }
}

public sealed record LoginCommandResult(string Token);