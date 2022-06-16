using MediatR;
using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Application.Requests.GetUserDetails;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.Identity.Models;

namespace SecureMessengerBohdan.Application.Requests.GetUsers
{
    public class GetUsersHandler : IRequestHandler<GetUsersRequest, List<GetUserDto>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CurrentUserService _currentUserService;
        private const int MaxUsers = 10;
        
        public GetUsersHandler(UserManager<ApplicationUser> userManager, CurrentUserService currentUserService)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetUserDto>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        {
            var userId = new Guid(_currentUserService.UserId);
            if (request.Search != null)
            {
                request.Search = request.Search.ToUpper();
            }
            return _userManager.Users.Where(user => user.Id != userId && 
            (string.IsNullOrEmpty(request.Search)
                || user.NormalizedUserName.StartsWith(request.Search)
                || user.NormalizedEmail.StartsWith(request.Search)))
                .Take(MaxUsers)
                .Select(user => new GetUserDto()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    UserName =  user.UserName,
                })
                .ToList();
        }
    }
}
