using EonData.CloudControl.AWS;
using EonData.DomainEntities.ContactForm.Models;

using Microsoft.VisualBasic;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;


namespace EonData.DomainLogic.ContactForm
{
    public class ContactFormService
    {
        public async Task SaveContactMessageAsync(ContactMessageModel message, CancellationToken cancellationToken)
        {
            using (AmazonDynamoDBClient dbClient = new())
            {
                await dbClient.PutItemAsync(new PutItemRequest()
                {
                    TableName = "EonDataContactForm",
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

        public Task ListContactMessagesAsync(CancellationToken cancellationToken) => null;// _storage.ListFilesAsync(bucketName, pathPrefix, cancellationToken);
    }
}
