using MediatR;
using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Identity.Models;
using System.Security.Claims;

namespace SecureMessengerBohdan.Application.Requests.GetUserDetails
{
    public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsRequest, GetUserDto>
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserDetailsHandler(IHttpContextAccessor accessor, UserManager<ApplicationUser> userManager)
        {
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<GetUserDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var id = _accessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                throw new ApplicationException("Claim user id is not found");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ApplicationException("User is not found");
            }
            
            return new GetUserDto()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName
            };
        }
    }
}
