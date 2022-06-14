using MediatR;
using SecureMessengerBohdan.Application.Services;

namespace SecureMessengerBohdan.Application.Requests.GetUserDetails
{
    public class GetUserDetailsHandler : IRequestHandler<GetUserDetailsRequest, GetUserDto>
    {
        private readonly CurrentUserService _currentUserService;

        public GetUserDetailsHandler(CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<GetUserDto> Handle(GetUserDetailsRequest request, CancellationToken cancellationToken)
        {
            var user = await _currentUserService.GetUser();

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
