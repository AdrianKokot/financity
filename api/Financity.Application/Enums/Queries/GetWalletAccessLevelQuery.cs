using Financity.Application.Enums.Queries.Abstract;
using Financity.Domain.Enums;

namespace Financity.Application.Enums.Queries;

public sealed class GetWalletAccessLevelQuery : GetEnumQuery<WalletAccessLevel>
{
}

public sealed class
    GetWalletAccessLevelQueryHandler : GetEnumQueryHandler<GetWalletAccessLevelQuery, WalletAccessLevel>
{
}