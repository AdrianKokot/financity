using Financity.Application.Abstractions.Messaging;
using Financity.Domain.Enums;

namespace Financity.Application.TransactionTypes.Queries;

public sealed class GetTransactionTypesQuery : IQuery<IEnumerable<TransactionTypeListItem>>
{
}

public sealed class
    GetTransactionTypesQueryHandler : IQueryHandler<GetTransactionTypesQuery, IEnumerable<TransactionTypeListItem>>
{
    public Task<IEnumerable<TransactionTypeListItem>> Handle(GetTransactionTypesQuery request,
                                                             CancellationToken cancellationToken)
    {
        var enumValueList = Enum.GetValues<TransactionType>()
                                .Select(x => new TransactionTypeListItem((int)x, x.ToString()));

        return Task.FromResult(enumValueList);
    }
}

public sealed record TransactionTypeListItem(int Id, string Name);