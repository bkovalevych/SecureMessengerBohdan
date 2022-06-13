using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureMessengerBohdan.Application.Requests.GetUserDetails;
using SecureMessengerBohdan.Application.Wrappers;
using SecureMessengerBohdan.Identity.Requests.Login;
using SecureMessengerBohdan.Identity.Requests.Refresh;
using SecureMessengerBohdan.Identity.Requests.Register;

namespace SecureMessengerBohdan.Controllers
{
    public class IdentityController : BaseController
    {
        public IdentityController(ISender sender) : base(sender)
        {
        }

        [HttpPost("login")]
        public async Task<GetTokenDto> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
        {
            return await Sender.Send(loginRequest, cancellationToken);
        }

        [HttpPost("register")]
        public async Task Register([FromBody] RegisterRequest registerRequest, CancellationToken cancellationToken)
        {
             await Sender.Send(registerRequest, cancellationToken);
        }

        [HttpPost("refresh")]
        public async Task<GetTokenDto> Refresh([FromBody] RefreshRequest refreshRequest, CancellationToken cancellationToken)
        {
            return await Sender.Send(refreshRequest, cancellationToken);
        }

        [Authorize]
        [HttpGet]
        public async Task<GetUserDto> GetUser(CancellationToken cancellationToken)
        {
            return await Sender.Send(new GetUserDetailsRequest(), cancellationToken);
        }
    }
}
