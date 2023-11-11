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
        public async Task<IActionResult> SendMessage(SendMessageModel message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                return BadRequest();
            }

            try
            {
                string rSource = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                await contactForm.SaveContactMessageAsync(message, rSource, cancellationToken);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
    }
}
