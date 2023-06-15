using ChicStreetwear.Application.Common.Interfaces;

namespace ChicStreetwear.Infrastructure.Services;
public sealed class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
