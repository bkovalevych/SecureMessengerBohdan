namespace SecureMessengerBohdan.Application.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() : base("base exception")
        {
        }

        public DomainException(params string[] messages)
        {
            Messages.AddRange(messages);
        }

        public List<string> Messages { get; set; } = new List<string>();
    }
}
