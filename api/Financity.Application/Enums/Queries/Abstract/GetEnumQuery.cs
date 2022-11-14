using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Enums.Queries.Abstract;

public abstract class GetEnumQuery<TEnum> : IQuery<IEnumerable<EnumListItem>> where TEnum : Enum
{
}

public abstract class
    GetEnumQueryHandler<TQuery, TEnum> : IQueryHandler<TQuery, IEnumerable<EnumListItem>>
    where TEnum : Enum where TQuery : GetEnumQuery<TEnum>
{
    public Task<IEnumerable<EnumListItem>> Handle(TQuery request,
                                                  CancellationToken cancellationToken)
    {
        var enumValueList = Enum.GetValues(typeof(TEnum))
                                .OfType<object>()
                                .Select(x => new EnumListItem((int)x, x.ToString()));

        return Task.FromResult(enumValueList);
    }
}

public sealed record EnumListItem(int Id, string Name);