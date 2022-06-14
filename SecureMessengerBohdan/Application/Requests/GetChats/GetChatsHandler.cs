using MediatR;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.Application.Requests.GetMessages;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.DataAccess;

namespace SecureMessengerBohdan.Application.Requests.GetChats
{
    public class GetChatsHandler : IRequestHandler<GetChatsRequest, List<GetChatDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly CurrentUserService _currentUserService;

        public GetChatsHandler(ApplicationDbContext context, CurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetChatDto>> Handle(GetChatsRequest request, CancellationToken cancellationToken)
        {
            var id = new Guid(_currentUserService.UserId);
            var project = Builders<Chat>.Projection;
            var filter = Builders<Chat>.Filter;
            var sort = Builders<Chat>.Sort;
            var pipeline = new EmptyPipelineDefinition<Chat>();
            PreloadTypes();

            var result = _context.ChatRecord
                .Aggregate()
                .Match(filter.ElemMatch(chat => chat.Members, it => it.Id == id))
                .Lookup<Chat, Message, Chat>(_context.MessageRecord,
                group => group.Id,
                message => message.ChatId,
                dto => dto.Messages)
                .Project(project.Expression(ch => new GetChatDto()
                {
                    Id = ch.Id,
                    Name = ch.Name,
                    LastMessage = ch.Messages
                    .Select(m => new GetMessageDto()
                    {
                        Id = m.Id,
                        FromId = m.FromId.ToString(),
                        FromName = m.FromName,
                        Text = m.Text,
                        Sent = m.Sent,
                    })
                    .FirstOrDefault(message => message.Sent == ch.Messages.Max(it => it.Sent))
                }))
                .ToList();
                
            //var result = _context.ChatRecord
            //    .AsQueryable()
            //    .Where(chat => chat.Members.Any(member => member.Id == id))
            //    .GroupJoin(_context.MessageRecord.AsQueryable(), chat => chat.Id, message => message.ChatId,
            //    (chat, messages) => new GetChatDto()
            //    {
            //        Id = chat.Id,
            //        Name = chat.Name,
            //        LastMessage = messages
            //        .Select(m => new GetMessageDto()
            //        {
            //            FromId = m.FromId.ToString(),
            //            FromName = m.FromName,
            //            Id = m.Id,
            //            Sent = m.Sent,
            //            Text = m.Text
            //        })
            //        .Aggregate(null,(a, b) => a.Sent < b.Sent ? b : a, a => )
            //    })

            //    .ToList();


            return result;
        }

        private void PreloadTypes()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(GetChatDto)))
            {
                BsonClassMap.RegisterClassMap<GetChatDto>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(GetMessageDto)))
            {
                BsonClassMap.RegisterClassMap<GetMessageDto>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}
