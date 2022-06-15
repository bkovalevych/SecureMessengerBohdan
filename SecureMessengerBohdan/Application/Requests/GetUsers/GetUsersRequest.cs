using MediatR;
using SecureMessengerBohdan.Application.RequestHelpers;
using SecureMessengerBohdan.Application.RequestHelpers.Interfaces;
using SecureMessengerBohdan.Application.Requests.GetUserDetails;

namespace SecureMessengerBohdan.Application.Requests.GetUsers
{
    public class GetUsersRequest : IRequest<List<GetUserDto>>, IHasPaging
    {
        public Paging? Paging { get; set; }

        public string Search { get; set; } 
    }
}
