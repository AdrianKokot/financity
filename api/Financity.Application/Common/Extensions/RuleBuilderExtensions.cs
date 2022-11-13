using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Financity.Application.Common.Extensions;

public static class RuleBuilderExtensions
{
    public static IRuleBuilder<T, string> MatchesIf<T>(this IRuleBuilder<T, string> ruleBuilder,
                                                       string matches, bool shouldMatch)
    {
        var result = ruleBuilder;

        if (shouldMatch) result = result.Matches(matches);

        return result;
    }

    public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder, PasswordOptions options)
    {
        return ruleBuilder.NotEmpty()
                          .MatchesIf("[A-Z]", options.RequireUppercase)
                          .MatchesIf("[a-z]", options.RequireLowercase)
                          .MatchesIf("[0-9]", options.RequireDigit)
                          .MatchesIf("[^a-zA-Z0-9]", options.RequireNonAlphanumeric)
                          .MinimumLength(options.RequiredLength)
                          .MaximumLength(int.MaxValue);
    }
}