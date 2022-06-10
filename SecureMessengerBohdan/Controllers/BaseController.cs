using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SecureMessengerBohdan.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        protected ISender Sender;

        public BaseController(ISender sender)
        {
            Sender = sender;
        }
    }
}
