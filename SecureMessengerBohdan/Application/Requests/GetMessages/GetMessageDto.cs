namespace SecureMessengerBohdan.Application.Requests.GetMessages
{
    public class GetMessageDto
    {
        public string Id { get; set; }

        public string Text { get; set; }

        public string ChatId { get; set; }

        public string? ChatName { get; set; }

        public string FromId { get; set; }

        public string? FromName { get; set; }

        public DateTimeOffset Sent { get; set; }
    }
}
