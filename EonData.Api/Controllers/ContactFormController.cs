using EonData.CloudControl.AWS;
using EonData.DomainEntities.ContactForm.Models;
using EonData.DomainLogic.ContactForm;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("contact")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        //[Authorize()]
        public IActionResult ListMessages(CancellationToken cancellationToken)
        {
            //_contactForm.ListContactMessagesAsync(cancellationToken);
            return Ok("asdf");
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessage(ContactMessageModel message)
        {
            if (message == null || (message.ContactName?.Length ?? 0) == 0 || (message.ContactAddress?.Length ?? 0) == 0 || (message.MessageContent?.Length ?? 0) == 0)
            {
                return BadRequest("Message data is missing.");
            }

            if (message.ContactName!.Length > 250 || message.ContactAddress!.Length > 250 || message.MessageContent!.Length > 7500)
            {
                return BadRequest("Message data exceeds limits.");
            }

            // store values needed for a new message record
            message.RequestSource = HttpContext.Connection.RemoteIpAddress?.ToString();
            message.MessageTimestamp = DateTime.UtcNow;

            try
            {
                ContactFormService contactForm = new();
                await contactForm.SaveContactMessageAsync(message, new CancellationToken());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
    }
}
