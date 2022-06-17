using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace SecureMessengerBohdan.Identity.Models
{
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public string? RefreshToken { get; set; }
        
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PublicKey { get; set; }
    }
}
