using ChicStreetwear.Domain.Common;

namespace ChicStreetwear.Domain.ValueObjects;

public sealed class Address : ValueObject
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public string AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string Country { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string PostalCode { get; private set; }

    private Address(string firstName, string lastName, string fullName, string email, string phoneNumber, string addressLine1, string? addressLine2, string country, string city, string state, string postalCode)
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2;
        Country = country;
        City = city;
        State = state;
        PostalCode = postalCode;
    }

    public static Address New(string firstName, string lastName, string fullName, string email, string phoneNumber, string addressLine1, string? addressLine2, string country, string city, string state, string postalCode)
    {
        return new(firstName, lastName, fullName, email, phoneNumber, addressLine1, addressLine2, country, city, state, postalCode);
    }

    public override IEnumerable<object> GetEqualtyComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return FullName;
        yield return Email;
        yield return PhoneNumber;
        yield return AddressLine1;
        yield return AddressLine2 ?? string.Empty;
        yield return Country;
        yield return City;
        yield return State;
        yield return PostalCode;
    }
}