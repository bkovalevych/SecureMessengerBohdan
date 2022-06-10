using MediatR;
using SecureMessengerBohdan.Identity.Requests.Login;

namespace SecureMessengerBohdan.Identity.Requests.Refresh
{
    public class RefreshRequest : IRequest<GetTokenDto>
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
