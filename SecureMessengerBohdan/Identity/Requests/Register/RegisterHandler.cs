using MediatR;
using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Identity.Models;

namespace SecureMessengerBohdan.Identity.Requests.Register
{
    public class RegisterHandler : IRequestHandler<RegisterRequest>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser()
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new ArgumentException($"Eror to create user. {string.Join('\n', result.Errors)}");
            }
            return Unit.Value;
        }
    }
}
