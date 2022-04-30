using Financity.Application.Common.Interfaces;

namespace Financity.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}