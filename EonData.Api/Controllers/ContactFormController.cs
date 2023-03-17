using EonData.CloudControl.AWS;
using EonData.DomainEntities.ContactForm.Models;
using EonData.DomainLogic.ContactForm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("contact")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly S3FileStorageService _storage;
        private readonly ContactFormService _contactForm;

        public ContactFormController(S3FileStorageService storage)
        {
            _storage = storage;
            _contactForm = new ContactFormService(storage);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessage(ContactMessageModel message)
        {   
            if (message == null || (message.Name?.Length ?? 0) == 0 || (message.ContactAddress?.Length ?? 0) == 0 || (message.Message?.Length ?? 0) == 0)
            {
                return BadRequest("Message data is missing.");
            }

            if (message.Name!.Length > 250 || message.ContactAddress!.Length > 250 || message.Message!.Length > 7500)
            {
                return BadRequest("Message data exceeds limits.");
            }

            try
            {
                await _contactForm.SaveContactMessageAsync(message, new CancellationToken());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
    }
}
