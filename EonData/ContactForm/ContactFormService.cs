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
        private const string CONTACT_MESSAGE_TABLE = "EonDataWebContactForm";

        private readonly IAmazonDynamoDB db;

        public ContactFormService(IAmazonDynamoDB dynamoDb) => db = dynamoDb;

        public async Task<int> GetTotalContactMessages(bool unreadOnly, CancellationToken cancellationToken)
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

            var result = await db.ScanAsync(totalCountRequest, cancellationToken);
            return result.Count;
        }

        public async Task SaveContactMessageAsync(SendMessageModel message, string requestSource, CancellationToken cancellationToken)
        {
            await db.PutItemAsync(new PutItemRequest()
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
            });
        }

        private async Task ListMessages(CancellationToken cancellationToken)
        {
            QueryRequest request = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                ProjectionExpression = "messageId, messageTimestamp, "
            };
        }
    }
}
