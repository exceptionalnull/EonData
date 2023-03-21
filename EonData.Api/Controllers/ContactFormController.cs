using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using EonData.DomainEntities.ContactForm.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("contact")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private IAmazonDynamoDB _dynamoDB;

        public ContactFormController(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessage(ContactMessageModel message)
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
                await SaveContactMessageAsync(message, new CancellationToken());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }

        private async Task SaveContactMessageAsync(ContactMessageModel message, CancellationToken cancellationToken)
        {
            await _dynamoDB.PutItemAsync(new PutItemRequest()
            {
                TableName = "EonDataWebContactForm",
                Item = new Dictionary<string, AttributeValue>()
                    {
                        { "messageId", new AttributeValue(Guid.NewGuid().ToString()) },
                        { "messageTimestamp", new AttributeValue(message.MessageTimestamp.ToString("s")) },
                        { "contactAddress", new AttributeValue(message.ContactAddress) },
                        { "contactName", new AttributeValue(message.ContactName) },
                        { "formSource", new AttributeValue(message.FormSource) },
                        { "requestSource", new AttributeValue(message.RequestSource) },
                        { "isRead", new AttributeValue() { BOOL = false } },
                        { "messageContent", new AttributeValue(message.MessageContent) }
                    }
            });
        }
    }
}
