using EonData.ContactForm.Models;
using EonData.ContactForm.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

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

        [HttpPost]
        [Route("")]
        [EnableRateLimiting("contactMessageLimit")]
        public async Task<IActionResult> SendMessage(SendMessageModel message, CancellationToken cancellationToken)
        {
            if (message == null)
            {
                return BadRequest("no message.");
            }

            try
            {
                string rSource = HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty;
                await contactForm.SaveContactMessageAsync(message, rSource, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }

        [HttpGet]
        [Route("total")]
        [Authorize]
        public async Task<ActionResult<int>> GetTotal(bool? unread, CancellationToken cancellationToken)
        {
            int messageCount = -1;
            try
            {
                messageCount = await contactForm.GetTotalContactMessagesAsync(unread, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok(messageCount);
        }

        [HttpGet]
        [Route("")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MessageListModel>>> ListMessages(bool? unread, CancellationToken cancellationToken)
        {
            IEnumerable<MessageListModel> result;
            try
            {
                result = await contactForm.ListMessagesAsync(unread, cancellationToken);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<ContactMessageModel>> GetMessage(Guid id, CancellationToken cancellationToken)
        {
            ContactMessageModel? message;
            try
            {
                message = await contactForm.GetContactMessageAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok(message);
        }

        [HttpPut]
        [Route("{id}/setread")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await contactForm.MarkAsReadAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
    }
}
