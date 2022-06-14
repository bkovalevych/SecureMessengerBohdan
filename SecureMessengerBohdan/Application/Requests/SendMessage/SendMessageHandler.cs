using MediatR;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Exceptions;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.Application.Requests.GetMessages;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.DataAccess;

namespace SecureMessengerBohdan.Application.Requests.SendMessage
{
    public class SendMessageHandler : IRequestHandler<SendMessageRequest, GetMessageDto>
    {
        private readonly CurrentUserService _currentUser;
        private readonly ApplicationDbContext _context;

        public SendMessageHandler(CurrentUserService currentUser, ApplicationDbContext applicationDbContext)
        {
            _currentUser = currentUser;
            _context = applicationDbContext;
        }

        public async Task<GetMessageDto> Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            var sender = await _currentUser.GetUser();
            var filter = Builders<Chat>.Filter;

            var receiver = await _context.ChatRecord.Find(filter.Eq(it => it.Id, request.ChatId))
            .FirstAsync(cancellationToken);


            if (receiver == null)
            {
                throw new DomainException("Receiver not found");
            }
            var message = new Message()
            {
                FromId = sender.Id,
                FromName = sender.Email,
                ChatId = receiver.Id.ToString(),
                Sent = DateTimeOffset.Now,
                ChatName = receiver.Name,
                Text = request.Text,
            };
            await _context.MessageRecord.InsertOneAsync(message);

            var messageDto = new GetMessageDto()
            {
                Id = message.Id,
                Sent = message.Sent,
                FromId = sender.Id.ToString(),
                FromName = sender.Email,
                ChatId = receiver.Id.ToString(),
                ChatName = receiver.Name,
                Text = request.Text,
            };
            return messageDto;
        }
    }
}
