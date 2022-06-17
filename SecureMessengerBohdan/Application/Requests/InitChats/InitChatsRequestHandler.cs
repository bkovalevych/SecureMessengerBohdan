using MediatR;
using Microsoft.AspNetCore.Identity;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.Application.Requests.GetChats;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.DataAccess;
using SecureMessengerBohdan.Identity.Models;
using SecureMessengerBohdan.Security.Requests.InitKeyForChat;

namespace SecureMessengerBohdan.Application.Requests.InitChats
{
    public class InitChatsRequestHandler : IRequestHandler<InitChatsRequest, List<Chat>>
    {
        private readonly ISender _sender;
        private readonly CurrentUserService _currentUser;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public InitChatsRequestHandler(CurrentUserService currentUser, ApplicationDbContext dbContext, 
            UserManager<ApplicationUser> userManager,
            ISender sender)
        {
            _sender = sender;
            _currentUser = currentUser;
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<List<Chat>> Handle(InitChatsRequest request, CancellationToken cancellationToken)
        {
            var user = await _currentUser.GetUser();
            var otherUsers = _userManager.Users.Where(u => u.Id != user.Id);
            var chats = new List<Chat>();
            foreach (var otherUser in otherUsers)
            {
                chats.Add(new Chat()
                {
                    Created = DateTimeOffset.Now,
                    Members = {user.Id.ToString(), otherUser.Id.ToString()},
                    Name = $"{user.UserName}, {otherUser.UserName}"
                });
            }
            await _dbContext.ChatRecord.InsertManyAsync(chats);
            
            
            
            return chats;
        }
    }
}
