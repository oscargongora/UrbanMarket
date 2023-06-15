namespace ChicStreetwear.Shared.Models.Order;
public class AddressModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string PostalCode { get; set; }

    public string AddressToString => $"{AddressLine1} {AddressLine2}, {City}, {State} {PostalCode}";

    public string ContactToString => FullName ?? $"{FirstName} {LastName}";
}
