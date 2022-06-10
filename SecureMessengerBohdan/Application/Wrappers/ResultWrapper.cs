namespace SecureMessengerBohdan.Application.Wrappers
{
    public class ResultWrapper
    {
        public ResultWrapper(params string[] errors)
        {
            Errors = new List<string>(errors);
        }

        public List<string> Errors { get; set; } = new List<string>();
        public bool IsSucceeded => !Errors.Any();

        public object? Result { set; get; } 
    }
}
