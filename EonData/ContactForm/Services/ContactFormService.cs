using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using EonData.ContactForm.Models;

namespace EonData.ContactForm.Services
{
    public class ContactFormService : IContactFormService
    {
        public const string CONTACT_MESSAGE_TABLE = "EonDataWebContactMessages";
        public const int READ_LIMIT = 50;

        private readonly IAmazonDynamoDB db;

        public ContactFormService(IAmazonDynamoDB dynamoDb) => db = dynamoDb;

        /// <summary>
        /// Gets a specific contact message.
        /// </summary>
        /// <param name="messageId">Message ID to retrieve.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>The message details or null if the ID does not exist.</returns>
        public async Task<ContactMessageModel?> GetContactMessageAsync(Guid messageId, CancellationToken cancellationToken)
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
                return new ContactMessageModel()
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

        /// <summary>
        /// Marks a message as having been read.
        /// </summary>
        /// <param name="messageId">Message to update the unread status of.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns></returns>
        public async Task MarkAsReadAsync(Guid messageId, CancellationToken cancellationToken)
        {
            UpdateItemRequest request = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Key = { { "messageId", new AttributeValue(messageId.ToString()) } },
                UpdateExpression = "SET isRead = :is_r",
                ExpressionAttributeValues = { { ":is_r", new AttributeValue() { BOOL = true } } },
                ConditionExpression = "attribute_exists(messageId)"
            };
            await db.UpdateItemAsync(request, cancellationToken);
        }

        /// <summary>
        /// Writes a contact message to the dynamodb table.
        /// </summary>
        /// <param name="message">Message details.</param>
        /// <param name="requestSource">Source of the message.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the count of contact messages. Can be filtered by unread status.
        /// </summary>
        /// <param name="unread">When null this will retrieve all messages. Specify true or false to retreive messages based on the unread status.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>Number of messages in the table.</returns>
        public async Task<int> GetTotalContactMessagesAsync(bool? unread, CancellationToken cancellationToken)
        {
            ScanRequest request = new(CONTACT_MESSAGE_TABLE)
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Select = Select.COUNT
            };

            // add filter if necessary
            if (unread != null)
            {
                request.FilterExpression = "#read = :read";
                request.ExpressionAttributeNames = new Dictionary<string, string>() { { "#read", "isRead" } };
                request.ExpressionAttributeValues = new Dictionary<string, AttributeValue>() { { ":read", new AttributeValue() { BOOL = !(bool)unread } } };
            }
            var result = await db.ScanAsync(request, cancellationToken);
            return result?.Count ?? 0;
        }

        /// <summary>
        /// Gets all of the contact messages
        /// </summary>
        /// <param name="unread">When null this will retrieve all messages. Specify true or false to retreive messages based on the unread status.</param>
        /// <param name="cancellationToken"><inheritdoc/></param>
        /// <returns>An object with the result messages and the start key value to use for the next page.</returns>
        public async Task<IEnumerable<MessageListModel>> ListMessagesAsync(bool? unread, CancellationToken cancellationToken)
        {
            string? lastEvaluatedKey = null;
            List<MessageListModel> messages = new();
            do
            {
                var result = await ReadMessagesAsync(unread, lastEvaluatedKey, cancellationToken);
                lastEvaluatedKey = result.Item2;
                if ((result.Item1?.Count() ?? 0) > 0)
                {
                    messages.AddRange(result.Item1!);
                }
            } while (lastEvaluatedKey != null);
            return messages;
        }

        // reads a batch of messages from the dynamodb table
        private async Task<Tuple<IEnumerable<MessageListModel>, string?>> ReadMessagesAsync(bool? unread, string? startKey, CancellationToken cancellationToken)
        {
            ScanRequest request = new()
            {
                TableName = CONTACT_MESSAGE_TABLE,
                Limit = READ_LIMIT,
                ProjectionExpression = "messageId, messageTimestamp, contactAddress, contactName, isRead"
            };

            // filter if necessary
            if (unread != null)
            {
                request.FilterExpression = "#read = :read";
                request.ExpressionAttributeNames = new Dictionary<string, string>() { { "#read", "isRead" } };
                request.ExpressionAttributeValues = new Dictionary<string, AttributeValue>() { { ":read", new AttributeValue() { BOOL = !(bool)unread } } };
            }

            // handle pagination
            if (startKey != null)
            {
                request.ExclusiveStartKey = new Dictionary<string, AttributeValue>() { { "messageId", new AttributeValue(startKey.ToString()) } };
            }

            var response = await db.ScanAsync(request, cancellationToken);

            // process results
            var messages = response.Items.Select<Dictionary<string, AttributeValue>, MessageListModel>(itm => new MessageListModel()
            {
                MessageId = new Guid(itm["messageId"].S),
                MessageTimestamp = DateTime.Parse(itm["messageTimestamp"].S),
                ContactAddress = itm["contactAddress"].S,
                ContactName = itm["contactName"].S,
                isRead = itm["isRead"].BOOL
            });
            string? lastEvaluatedKey = (response.LastEvaluatedKey.ContainsKey("messageId")) ? response.LastEvaluatedKey["messageId"].S : null;
            return Tuple.Create(messages, lastEvaluatedKey);
        }
    }
}