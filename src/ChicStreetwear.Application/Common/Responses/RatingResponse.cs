using ChicStreetwear.Domain.ValueObjects;

namespace ChicStreetwear.Application.Common.Responses;
public sealed class RatingResponse
{
    public float Value { get; private set; }
    public int Count { get; private set; }
    public static RatingResponse FromRating(Rating rating)
    {
        return new()
        {
            Value = rating.Value,
            Count = rating.Count
        };
    }
}