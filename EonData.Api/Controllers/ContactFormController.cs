using EonData.ContactForm;
using EonData.ContactForm.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("contact")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private IContactFormService contactForm;

        public ContactFormController(IContactFormService contactFormService)
        {
            contactForm = contactFormService;
        }

        [HttpGet]
        [Route("total")]
        [Authorize]
        public async Task<IActionResult> GetTotal(bool unread, CancellationToken cancellationToken)
        {
            int messageCount = -1;
            try
            {
                messageCount = await contactForm.GetTotalContactMessages(unread, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(messageCount);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> ListMessages(CancellationToken cancellationToken)
        {
            return Ok("testing 1 2 3");
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessage(ContactMessageModel message, CancellationToken cancellationToken)
        {
            if (message == null || (message.ContactName?.Length ?? 0) == 0 || (message.ContactAddress?.Length ?? 0) == 0 || (message.MessageContent?.Length ?? 0) == 0 || (message.FormSource?.Length ?? 0) == 0)
            {
                return BadRequest("Message data is missing.");
            }

            if (message.ContactName!.Length > 250 || message.ContactAddress!.Length > 250 || message.MessageContent!.Length > 7500 || message.FormSource!.Length > 10)
            {
                return BadRequest("Message data exceeds limits.");
            }

            // store values needed for a new message record
            message.RequestSource = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
            message.MessageTimestamp = DateTime.UtcNow;

            try
            {
                await contactForm.SaveContactMessageAsync(message, cancellationToken);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
    }
}
