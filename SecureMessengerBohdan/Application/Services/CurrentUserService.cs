using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Application.Exceptions;
using SecureMessengerBohdan.Identity.Models;
using System.Security.Claims;

namespace SecureMessengerBohdan.Application.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, 
            UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public ClaimsPrincipal? User { get; set; }

        public string? UserId => _httpContextAccessor
            .HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        public async Task<ApplicationUser> GetUser()
        {
            var id = (_httpContextAccessor.HttpContext?.User ?? User)
                .FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                throw new DomainException("User id missed");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new DomainException("User was not found");
            }

            return user;
        }
    }
}
