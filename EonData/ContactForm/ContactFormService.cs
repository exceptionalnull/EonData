using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace EonData.ContactForm
{
    public class ContactFormService : IContactFormService
    {
        private const string CONTACT_MESSAGE_TABLE = "EonDataWebContactMessages";

        private readonly IAmazonDynamoDB db;

        public ContactFormService(IAmazonDynamoDB dynamoDb) => db = dynamoDb;

        public async Task<ContactMessage?> GetContactMessageAsync(Guid messageId, CancellationToken cancellationToken)
        {
            GetItemRequest request = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Key = { { "messageId", new AttributeValue(messageId.ToString()) } },
                ProjectionExpression = "messageId, messageTimestamp, contactAddress, contactName, formSource, requestSource, isRead, messageContent"
            };

            var result = await db.GetItemAsync(request, cancellationToken);
            if (result.IsItemSet)
            {
                return new ContactMessage()
                {
                    MessageId = new Guid(result.Item["messageId"].S),
                    MessageTimestamp = DateTime.Parse(result.Item["messageTimestamp"].S),
                    ContactAddress = result.Item["contactAddress"].S,
                    ContactName = result.Item["contactName"].S,
                    FormSource = result.Item["formSource"].S,
                    RequestSource = result.Item["requestSource"].S,
                    isRead = result.Item["isRead"].BOOL,
                    MessageContent = result.Item["messageContent"].S
                };
            }
            return null;
        }

        public async Task MarkAsReadAsync(Guid messageId, CancellationToken cancellationToken)
        {
            UpdateItemRequest request = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Key = { { "messageId", new AttributeValue(messageId.ToString()) } },
                ExpressionAttributeNames = { { "#R", "isRead" } },
                UpdateExpression = "SET #R = :is_r",
                ExpressionAttributeValues = { { ":is_r", new AttributeValue() { BOOL = true } } }
            };
            await db.UpdateItemAsync(request, cancellationToken);
        }

        public async Task SaveContactMessageAsync(SendMessageModel message, string requestSource, CancellationToken cancellationToken)
        {
            PutItemRequest request = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Item = new Dictionary<string, AttributeValue>()
                    {
                        { "messageId", new AttributeValue(Guid.NewGuid().ToString()) },
                        { "messageTimestamp", new AttributeValue(DateTime.UtcNow.ToString("s")) },
                        { "contactAddress", new AttributeValue(message.ContactAddress) },
                        { "contactName", new AttributeValue(message.ContactName) },
                        { "formSource", new AttributeValue(message.FormSource) },
                        { "requestSource", new AttributeValue(requestSource) },
                        { "isRead", new AttributeValue() { BOOL = false } },
                        { "messageContent", new AttributeValue(message.MessageContent) }
                    }
            };
            await db.PutItemAsync(request, cancellationToken);
        }

        public async Task<int> GetTotalContactMessagesAsync(bool unreadOnly, CancellationToken cancellationToken)
        {
            ScanRequest totalRequest = getScan(unreadOnly);
            totalRequest.Select = Select.COUNT;

            var result = await db.ScanAsync(totalRequest, cancellationToken);
            return result.Count;
        }

        public async Task<IEnumerable<MessageListModel>> ListMessagesAsync(bool unreadOnly, CancellationToken cancellationToken)
        {
            ScanRequest request = getScan(unreadOnly);
            request.Select = Select.SPECIFIC_ATTRIBUTES;
            request.AttributesToGet = new() { "messageId", "messageTimestamp", "contactAddress", "contactName", "isRead" };

            var response = await db.ScanAsync(request, cancellationToken);

            return response.Items.Select<Dictionary<string, AttributeValue>, MessageListModel>(x => new MessageListModel()
            {
                MessageId = new Guid(x["messageId"].S),
                MessageTimestamp = DateTime.Parse(x["messageTimestamp"].S),
                ContactAddress = x["contactAddress"].S,
                ContactName = x["contactName"].S,
                isRead = x["isRead"].BOOL
            });
        }

        private ScanRequest getScan(bool unreadOnly)
        {
            ScanRequest request = new(CONTACT_MESSAGE_TABLE);
            if (unreadOnly)
            {
                request.FilterExpression = "#read = :read";
                request.ExpressionAttributeNames = new Dictionary<string, string>() { { "#read", "isRead" } };
                request.ExpressionAttributeValues = new Dictionary<string, AttributeValue>() { { ":read", new AttributeValue() { BOOL = false } } };
            }
            return request;
        }
    }
}
