using Financity.Application.Enums.Queries.Abstract;
using Financity.Domain.Enums;

namespace Financity.Application.Enums.Queries;

public sealed class GetTransactionTypeQuery : GetEnumQuery<TransactionType>
{
}

public sealed class
    GetTransactionTypeQueryHandler : GetEnumQueryHandler<GetTransactionTypeQuery, TransactionType>
{
}