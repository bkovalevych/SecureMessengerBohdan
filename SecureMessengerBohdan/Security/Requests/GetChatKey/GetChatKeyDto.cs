namespace SecureMessengerBohdan.Security.Requests.GetChatKey
{
    public class GetChatKeyDto
    {
        public string ChatId { get; set; }
        public string Key { get; set; }
        public string IV { get; set; }
    }
}
