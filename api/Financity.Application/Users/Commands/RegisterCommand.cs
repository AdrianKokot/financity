﻿using AutoMapper;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Common.Mappings;
using Financity.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Users.Commands;

public sealed record RegisterCommand(string Email, string Password) : ICommand<RegisterCommandResult>;

public sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, RegisterCommandResult>
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public RegisterCommandHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<RegisterCommandResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new User {Email = request.Email, UserName = request.Email};
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(x => new ValidationFailure(x.Code, x.Description)));

        return _mapper.Map<RegisterCommandResult>(user);
    }
}

public sealed record RegisterCommandResult : IMapFrom<User>;