using MediatR;
using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Identity.Helpers;
using SecureMessengerBohdan.Identity.Models;

namespace SecureMessengerBohdan.Identity.Requests.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, GetTokenDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly TokenWriter _tokenWriter;

        public LoginHandler(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            TokenWriter tokenWriter)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenWriter = tokenWriter;
        }

        public async Task<GetTokenDto> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ArgumentException("User does not exist");
            }
            var signInResult = await _signInManager.PasswordSignInAsync(user, request.Password, false, false);
            if (!signInResult.Succeeded)
            {
                throw new ArgumentException("Password is incorrect");
            }
            if (string.IsNullOrEmpty(user.RefreshToken))
            {
                user.RefreshToken = _tokenWriter.GenerateRefreshToken();
                await _userManager.UpdateAsync(user);
            }
            var accessToekn = _tokenWriter.WriteAccessToken(user);
            
            return new GetTokenDto()
            {
                AccessToken = accessToekn,
                RefreshToken = user.RefreshToken
            };
        }
    }
}
