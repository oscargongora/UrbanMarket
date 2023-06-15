using Microsoft.AspNetCore.Identity;

namespace ChicStreetwear.Shared.Identity.Models;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}
