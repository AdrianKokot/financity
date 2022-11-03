using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Enums;

namespace Financity.Application.TransactionTypes.Queries;

public sealed class GetTransactionTypesQuery : IQuery<IEnumerable<TransactionTypeListItem>>
{
}

public sealed class
    GetTransactionTypesQueryHandler : IQueryHandler<GetTransactionTypesQuery, IEnumerable<TransactionTypeListItem>>
{
    public async Task<IEnumerable<TransactionTypeListItem>> Handle(GetTransactionTypesQuery request,
                                                                   CancellationToken cancellationToken)
    {
        return Enum.GetValues<TransactionType>().Select(x => new TransactionTypeListItem((int) x, x.ToString()))
                   .ToList();
    }
}

public sealed record TransactionTypeListItem(int Id, string Name);