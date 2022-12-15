using Financity.Application.Abstractions.Messaging;

namespace Financity.Application.Enums.Queries.Abstract;

public interface IGetEnumQuery<TEnum> : IQuery<IEnumerable<string>> where TEnum : Enum
{
}

public abstract class
    GetEnumQueryHandler<TQuery, TEnum> : IQueryHandler<TQuery, IEnumerable<string>>
    where TEnum : Enum where TQuery : IGetEnumQuery<TEnum>
{
    public virtual Task<IEnumerable<string>> Handle(TQuery request,
                                                    CancellationToken cancellationToken)
    {
        var enumValueList = Enum.GetValues(typeof(TEnum))
                                .OfType<object>()
                                .Select(x => x.ToString() ?? string.Empty);

        return Task.FromResult(enumValueList);
    }
}