using Microsoft.AspNetCore.Identity;

namespace GymTracker.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser
{
	public string? DisplayName { get; set; }
}