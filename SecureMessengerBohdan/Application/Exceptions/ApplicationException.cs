namespace SecureMessengerBohdan.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException() : base("base exception")
        {
        }

        public ApplicationException(params string[] messages)
        {
            Messages.AddRange(messages);
        }

        public List<string> Messages { get; set; } = new List<string>();
    }
}
