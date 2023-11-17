using EonData.ContactForm.Models;
using EonData.ContactForm.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> ListMessages(bool? unread, CancellationToken cancellationToken)
        {
            IEnumerable<MessageListModel> messages;
            try
            {
                messages = await contactForm.ListMessagesAsync(unread ?? false, cancellationToken);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
            return Ok(messages);
        }

        [HttpPost]
        [Route("")]
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
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }

        [HttpGet]
        [Route("message")]
        [Authorize]
        public async Task<IActionResult> GetMessage(Guid id, CancellationToken cancellationToken)
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
        [Route("setread")]
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
