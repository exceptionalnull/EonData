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
        private const string CONTACT_MESSAGE_TABLE = "EonDataWebContactForm";
        private IAmazonDynamoDB _dynamoDB;

        public ContactFormController(IAmazonDynamoDB dynamoDB)
        {
            _dynamoDB = dynamoDB;
        }

        [HttpGet]
        [Route("total")]
        [Authorize]
        public async Task<IActionResult> GetTotal(bool unread, CancellationToken cancellationToken)
        {
            int messageCount = -1;
            try
            {
                messageCount = await GetTotalContactMessages(unread, cancellationToken);
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
                await SaveContactMessageAsync(message, cancellationToken);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }

        private async Task<int> GetTotalContactMessages(bool unreadOnly, CancellationToken cancellationToken)
        {
            ScanRequest totalCountRequest = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Select = Select.COUNT
            };

            if (unreadOnly)
            {
                totalCountRequest.FilterExpression = "#read = :read";
                totalCountRequest.ExpressionAttributeNames = new Dictionary<string, string>() { { "#read", "isRead" } };
                totalCountRequest.ExpressionAttributeValues = new Dictionary<string, AttributeValue>() { { ":read", new AttributeValue() { BOOL = false } } };
            }

            var result = await _dynamoDB.ScanAsync(totalCountRequest, cancellationToken);
            return result.Count;
        }

        private async Task SaveContactMessageAsync(ContactMessageModel message, CancellationToken cancellationToken)
        {
            await _dynamoDB.PutItemAsync(new PutItemRequest()
            {
                TableName = CONTACT_MESSAGE_TABLE,
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
