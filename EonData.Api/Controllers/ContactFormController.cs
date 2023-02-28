using EonData.DomainEntities.ContactForm.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        [HttpGet]
        public IActionResult SendMessage(ContactMessageModel message)
        {

            return Ok();
        }
    }
}
