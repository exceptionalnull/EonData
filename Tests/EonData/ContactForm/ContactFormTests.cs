using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.S3.Model;

using EonData.ContactForm.Models;
using EonData.ContactForm.Services;

using Moq;

namespace Tests.EonData.ContactForm
{
    public class ContactFormTests
    {
        [Fact]
        public async Task CanWriteNewContactMessage()
        {
            SendMessageModel sendMessage = new()
            {
                ContactAddress = "testing@example.com",
                ContactName = "Test Suite",
                FormSource = "xunit",
                MessageContent = "this is a test message."
            };

            var mockDynamoDB = new Mock<IAmazonDynamoDB>();
            mockDynamoDB.Setup(dydb => dydb.PutItemAsync(It.IsAny<PutItemRequest>(), It.IsAny<CancellationToken>()))
                .Callback<PutItemRequest, CancellationToken>((req, _) =>
                {
                    Assert.True(req.Item.ContainsKey("messageId"));
                    Assert.NotNull(req.Item["messageId"].S);
                    Assert.NotEmpty(req.Item["messageId"].S);

                    Assert.True(req.Item.ContainsKey("messageTimestamp"));
                    Assert.NotNull(req.Item["messageTimestamp"].S);
                    Assert.NotEmpty(req.Item["messageTimestamp"].S);

                    Assert.True(req.Item.ContainsKey("contactAddress"));
                    Assert.NotNull(req.Item["contactAddress"].S);
                    Assert.Equal(sendMessage.ContactAddress, req.Item["contactAddress"].S);

                    Assert.True(req.Item.ContainsKey("contactName"));
                    Assert.NotNull(req.Item["contactName"].S);
                    Assert.Equal(sendMessage.ContactName, req.Item["contactName"].S);

                    Assert.True(req.Item.ContainsKey("formSource"));
                    Assert.NotNull(req.Item["formSource"].S);
                    Assert.Equal(sendMessage.FormSource, req.Item["formSource"].S);

                    Assert.True(req.Item.ContainsKey("requestSource"));
                    Assert.NotNull(req.Item["requestSource"].S);
                    Assert.Equal("255.255.255.0", req.Item["requestSource"].S);

                    Assert.True(req.Item.ContainsKey("isRead"));
                    Assert.False(req.Item["isRead"].BOOL, "New message should be set to unread.");

                    Assert.True(req.Item.ContainsKey("messageContent"));
                    Assert.NotNull(req.Item["messageContent"].S);
                    Assert.Equal(sendMessage.MessageContent, req.Item["messageContent"].S);
                });
            var service = new ContactFormService(mockDynamoDB.Object);
            await service.SaveContactMessageAsync(sendMessage, "255.255.255.0", new CancellationToken());
        }

        [Fact]
        public async Task CanReadExistingContactMessage()
        {
            SendMessageModel sentMessage = new()
            {
                ContactAddress = "testing@example.com",
                ContactName = "Test Suite",
                FormSource = "xunit",
                MessageContent = "this is a test message."
            };

            var mockDynamoDB = new Mock<IAmazonDynamoDB>();
            mockDynamoDB.Setup(dydb => dydb.GetItemAsync(It.IsAny<GetItemRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(new GetItemResponse()
            {
                IsItemSet = true,
                Item = new Dictionary<string, AttributeValue>()
                {
                    { "messageId", new AttributeValue(Guid.Empty.ToString()) },
                    { "messageTimestamp", new AttributeValue(new DateTime(2022, 2, 2, 2, 22, 22).ToString("s")) },
                    { "contactAddress", new AttributeValue("testing@example.com") },
                    { "contactName", new AttributeValue("Testing Suite") },
                    { "formSource", new AttributeValue("xunit") },
                    { "requestSource", new AttributeValue("255.255.255.0") },
                    { "isRead", new AttributeValue() { BOOL = false } },
                    { "messageContent", new AttributeValue("this is a test record") }
                }
            });

            var service = new ContactFormService(mockDynamoDB.Object);
            var result = await service.GetContactMessageAsync(Guid.Empty, new CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(Guid.Empty, result.MessageId);
            Assert.Equal(new DateTime(2022, 2, 2, 2, 22, 22), result.MessageTimestamp);
            Assert.Equal("testing@example.com", result.ContactAddress);
            Assert.Equal("Testing Suite", result.ContactName);
            Assert.Equal("xunit", result.FormSource);
            Assert.Equal("255.255.255.0", result.RequestSource);
            Assert.False(result.isRead);
            Assert.Equal("this is a test record", result.MessageContent);
        }
    }
}