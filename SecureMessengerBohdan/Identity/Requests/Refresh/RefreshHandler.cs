using MediatR;
using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Identity.Helpers;
using SecureMessengerBohdan.Identity.Models;
using SecureMessengerBohdan.Identity.Requests.Login;
using System.Security.Claims;

namespace SecureMessengerBohdan.Identity.Requests.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshRequest, GetTokenDto>
    {
        private readonly TokenWriter _writer;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshHandler(TokenWriter writer, UserManager<ApplicationUser> userManager)
        {
            _writer = writer;
            _userManager = userManager;
        }

        public async Task<GetTokenDto> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            var principal = _writer.GetPrincipalFromExpiredToken(request.AccessToken);
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            {
                throw new ArgumentException("Id was not defined");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User was not found");
            }
            if (user.RefreshToken != request.RefreshToken || request.RefreshToken == null)
            {
                throw new ArgumentException("RefreshToken mismatched");
            }
            var refreshToken = _writer.GenerateRefreshToken();
            var accessToken = _writer.WriteAccessToken(user);
            
            return new GetTokenDto()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
