using MediatR;
using SecureMessengerBohdan.Application.Models;

namespace SecureMessengerBohdan.Application.Requests.InitChats
{
    public class InitChatsRequest : IRequest<List<Chat>>
    {

    }
}
