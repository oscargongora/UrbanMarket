using ChicStreetwear.Application.Common.Interfaces.Repositories;
using ChicStreetwear.Application.Common.Mappers;
using ChicStreetwear.Application.Common.Queries;
using ChicStreetwear.Domain.Aggregates.User;
using ChicStreetwear.Shared;
using ChicStreetwear.Shared.Models.Components;
using MediatR;
using System.Linq.Expressions;

namespace ChicStreetwear.Application.Users.Queries;

public sealed class GetUsersQuery : GetQueryBase, IRequest<Result<PaginatedListModel<UserResponse>>>
{
    public string? name { get; set; } = default;
    public string? address { get; set; } = default;
    public string? country { get; set; } = default;
    public string? city { get; set; } = default;
    public string? state { get; set; } = default;
    public string? postalCode { get; set; } = default;
}

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<PaginatedListModel<UserResponse>>>
{
    private readonly IRepository<User> _userRepository;

    public GetUsersQueryHandler(IRepository<User> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<PaginatedListModel<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        List<Expression<Func<User, bool>>> predicates = new();

        #region predicates
        if (request.name is not null)
        {
            predicates.Add(u => u.Addresses.Any(a => a.FirstName.Contains(request.name) || a.LastName.Contains(request.name)));
        }
        if (request.address is not null)
        {
            predicates.Add(u => u.Addresses.Any(a => a.AddressLine1.Contains(request.address) || (!string.IsNullOrEmpty(a.AddressLine2) && a.AddressLine2.Contains(request.address))));
        }
        if (request.country is not null)
        {
            predicates.Add(u => u.Addresses.Any(a => a.Country.Contains(request.country)));
        }
        if (request.city is not null)
        {
            predicates.Add(u => u.Addresses.Any(a => a.City.Contains(request.city)));
        }
        if (request.state is not null)
        {
            predicates.Add(u => u.Addresses.Any(a => a.State.Contains(request.state)));
        }
        if (request.postalCode is not null)
        {
            predicates.Add(u => u.Addresses.Any(a => a.PostalCode.Contains(request.postalCode)));
        }
        #endregion

        var users = await _userRepository.PaginatedListAsync(predicates, new(), request.Page ?? 1, request.Take ?? 10, true, cancellationToken);

        return Result<PaginatedListModel<UserResponse>>.Succeeded(new(users.Items.ConvertAll(u => u.ToUserResponse()), users.TotalItems));
    }
}
