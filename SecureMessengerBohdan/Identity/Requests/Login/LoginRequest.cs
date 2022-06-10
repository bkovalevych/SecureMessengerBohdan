using MediatR;

namespace SecureMessengerBohdan.Identity.Requests.Login
{
    public class LoginRequest : IRequest<GetTokenDto>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
