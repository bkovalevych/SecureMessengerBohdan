namespace SecureMessengerBohdan.Identity.Requests.Login
{
    public class GetTokenDto
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
