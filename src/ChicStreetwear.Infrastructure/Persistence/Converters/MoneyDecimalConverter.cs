using ChicStreetwear.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace ChicStreetwear.Infrastructure.Persistence.Converters;
internal class MoneyDecimalConverter : ValueConverter<Money?, decimal?>
{
    private static readonly Expression NullMoneyExpression = Expression.Constant(null, typeof(Money));
    private static readonly Expression NullDecimalExpression = Expression.Constant(null, typeof(decimal));
    public MoneyDecimalConverter()
        : base(convertTo => Expression.Equals(convertTo, NullMoneyExpression) ? null : convertTo!.Amount,
               convertFrom => Expression.Equals(convertFrom, NullDecimalExpression) ?
               null :
               Money.New((decimal)convertFrom!))
    {

    }
}
