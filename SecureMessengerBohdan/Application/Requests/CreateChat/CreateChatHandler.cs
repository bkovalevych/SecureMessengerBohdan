using MediatR;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.Application.Requests.GetChats;
using SecureMessengerBohdan.DataAccess;

namespace SecureMessengerBohdan.Application.Requests.CreateChat
{
    public class CreateChatHandler : IRequestHandler<CreateChatRequest, GetChatDto>
    {
        private readonly ApplicationDbContext _context;

        public CreateChatHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GetChatDto> Handle(CreateChatRequest request, CancellationToken cancellationToken)
        {
            var chat = new Chat()
            {
                Created = DateTimeOffset.Now,
                Members = request.MemberIds,
                Name = request.ChatName
            };
            await _context.ChatRecord.InsertOneAsync(chat, new InsertOneOptions() 
            {
                BypassDocumentValidation = false
            }, cancellationToken);
            
            return new GetChatDto()
            {
                Id = chat.Id,
                Name = chat.Name
            };
        }
    }
}
