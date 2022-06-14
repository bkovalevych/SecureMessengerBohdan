using MediatR;
using MongoDB.Driver;
using SecureMessengerBohdan.Application.Models;
using SecureMessengerBohdan.Application.RequestHelpers;
using SecureMessengerBohdan.Application.Services;
using SecureMessengerBohdan.Application.Wrappers;
using SecureMessengerBohdan.DataAccess;

namespace SecureMessengerBohdan.Application.Requests.GetMessages
{
    public class GetMessagesHandler : IRequestHandler<GetMessagesRequest, PagingList<GetMessageDto>>
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly CurrentUserService _currentUserService;

        public GetMessagesHandler(ApplicationDbContext appContext, CurrentUserService currentUserService)
        {
            _appDbContext = appContext;
            _currentUserService = currentUserService;
        }

        public async Task<PagingList<GetMessageDto>> Handle(GetMessagesRequest request, CancellationToken cancellationToken)
        {
            var id = new Guid(_currentUserService.UserId);
            var project = Builders<Message>.Projection;
            var filter = Builders<Message>.Filter.Eq(message => message.ChatId, request.ChatId);
            var sort = Builders<Message>.Sort;
            if (request.Paging == null)
            {
                request.Paging = new Paging()
                {
                    Skip = 0,
                    Take = 50,
                };
            }
            var items = await _appDbContext.MessageRecord
                .Aggregate()
                .Match(filter)
                .Project(project.Expression(m => new GetMessageDto()
                {
                    Id = m.Id,
                    FromId = m.FromId.ToString(),
                    ChatId = m.ChatId,
                    ChatName = m.ChatName,
                    FromName = m.FromName,
                    Sent = m.Sent,
                    Text = m.Text,
                }))
                .Limit(request.Paging.Take)
                .Skip(request.Paging.Skip)
                .ToListAsync();
            var result = new PagingList<GetMessageDto>()
            {
                Items = items,
                Skip = request.Paging.Skip,
                Take = items.Count,
                TotalCount = await _appDbContext.MessageRecord.Find(filter).CountDocumentsAsync()
            };
            return result;
        }
    }
}
