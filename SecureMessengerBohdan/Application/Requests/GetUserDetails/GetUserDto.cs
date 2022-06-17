namespace SecureMessengerBohdan.Application.Requests.GetUserDetails
{
    public class GetUserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PublicKey { get; set; }
    }
}
