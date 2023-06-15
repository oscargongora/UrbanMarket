using ChicStreetwear.Domain.ValueObjects;

namespace ChicStreetwear.Application.Common.Commands;
public sealed record AddressCommand(string firstName, string lastName, string fullName, string email, string phoneNumber, string addressLine1, string? addressLine2, string country, string city, string state, string postalCode)
{
    internal Address ToAddress => Address.New(firstName, lastName, fullName, email, phoneNumber, addressLine1, addressLine2, country, city, state, postalCode);
}