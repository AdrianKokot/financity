using AutoMapper;
using Financity.Application.Abstractions.Data;
using Financity.Application.Abstractions.Messaging;
using Financity.Application.Auth.Queries;
using Financity.Application.Common.Helpers;
using Financity.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Auth.Commands;

public sealed record UpdateUserCommand(string Name) : ICommand<UserDetails>;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserDetails>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UpdateUserCommandHandler(UserManager<User> userManager, IMapper mapper,
                                    ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<UserDetails> Handle(UpdateUserCommand request, CancellationToken ct)
    {
        var user = await _userManager.FindByEmailAsync(_currentUserService.NormalizedUserEmail)
                   ?? throw ValidationExceptionFactory.For(nameof(request.Name), "Given user doesn't exist.");

        user.Name = request.Name;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            throw new ValidationException(result.Errors.Select(x =>
                new ValidationFailure(nameof(request.Name), x.Description)));

        return _mapper.Map<UserDetails>(user);
    }
}