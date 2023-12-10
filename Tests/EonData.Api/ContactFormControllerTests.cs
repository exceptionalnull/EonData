using EonData.Api.Controllers;
using EonData.ContactForm.Models;
using EonData.ContactForm.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

using Moq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tests.EonData.Api
{
    public class ContactFormControllerTests
    {
        [Fact]
        public async Task CanSendMessage()
        {
            SendMessageModel sendMessage = new() {
                ContactAddress = "testing@example.com",
                ContactName = "Unit Test",
                FormSource = "xunit",
                MessageContent = "this is a test message."
            };

            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.SaveContactMessageAsync(It.IsAny<SendMessageModel>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Callback<SendMessageModel, string, CancellationToken>((msg, src, _) =>
                {
                    Assert.Equal(sendMessage, msg);
                    Assert.Equal("255.255.255.0", src);
                });
            var controller = new ContactFormController(mockService.Object);
            var context = new DefaultHttpContext();
            context.Connection.RemoteIpAddress = IPAddress.Parse("255.255.255.0"); // Set an IP address
            controller.ControllerContext = new ControllerContext() { HttpContext = context };

            var response = await controller.SendMessage(sendMessage, new CancellationToken());

            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);
            mockService.Verify(cfrm => cfrm.SaveContactMessageAsync(It.IsAny<SendMessageModel>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CanGetTotal()
        {
            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.GetTotalContactMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .Callback<bool?, CancellationToken>((unread, _) => { Assert.Null(unread); })
                .ReturnsAsync(11);
            var controller = new ContactFormController(mockService.Object);
            var response = await controller.GetTotal(null, new CancellationToken());
            Assert.NotNull(response);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(11, okResult.Value);
            mockService.Verify(cfrm => cfrm.GetTotalContactMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CanGetTotalUnread()
        {
            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.GetTotalContactMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .Callback<bool?, CancellationToken>((unread, _) => { Assert.True(unread); })
                .ReturnsAsync(7);
            var controller = new ContactFormController(mockService.Object);
            var response = await controller.GetTotal(true, new CancellationToken());
            Assert.NotNull(response);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(7, okResult.Value);
            mockService.Verify(cfrm => cfrm.GetTotalContactMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CanGetTotalRead()
        {
            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.GetTotalContactMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .Callback<bool?, CancellationToken>((unread, _) => { Assert.False(unread); })
                .ReturnsAsync(3);
            var controller = new ContactFormController(mockService.Object);
            var response = await controller.GetTotal(false, new CancellationToken());
            Assert.NotNull(response);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(3, okResult.Value);
            mockService.Verify(cfrm => cfrm.GetTotalContactMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CanListMessages()
        {
            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.ListMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()))
                .Callback<bool?, CancellationToken>((unread, _) => { Assert.Null(unread); })
                .ReturnsAsync(new List<MessageListModel>());
            var controller = new ContactFormController(mockService.Object);
            var response = await controller.ListMessages(null, new CancellationToken());
            Assert.NotNull(response);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.NotNull(okResult.Value);
            Assert.IsAssignableFrom<IEnumerable<MessageListModel>>(okResult.Value);
            mockService.Verify(cfrm => cfrm.ListMessagesAsync(It.IsAny<bool?>(), It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public async Task CanGetMessage()
        {
            Guid testMessageId = Guid.NewGuid();
            ContactMessageModel testMessage = new()
            {
                MessageId = testMessageId,
                ContactAddress = "testing@example.com",
                ContactName = "Unit Test",
                FormSource = "xunit",
                RequestSource = "255.255.255.0",
                isRead = true,
                MessageContent = "this is a test message.",
                MessageTimestamp = DateTime.UtcNow
            };

            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.GetContactMessageAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Callback<Guid, CancellationToken>((mid, _) => { Assert.Equal(testMessageId, mid); })
                .ReturnsAsync(testMessage);
            var controller = new ContactFormController(mockService.Object);
            var response = await controller.GetMessage(testMessageId, new CancellationToken());
            Assert.NotNull(response);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            Assert.Equal(testMessage, okResult.Value);
            mockService.Verify(cfrm => cfrm.GetContactMessageAsync(testMessageId, It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async Task CanSetMessageAsRead()
        {
            Guid testMessageId = Guid.NewGuid();
            var mockService = new Mock<IContactFormService>();
            mockService.Setup(cfrm => cfrm.MarkAsReadAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .Callback<Guid, CancellationToken>((mid, _) => { Assert.Equal(testMessageId, mid); });
            var controller = new ContactFormController(mockService.Object);
            var response = await controller.MarkAsRead(testMessageId, new CancellationToken());
            Assert.NotNull(response);
            Assert.IsType<OkResult>(response);
            mockService.Verify(cfrm => cfrm.MarkAsReadAsync(testMessageId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Fact]
        public void CheckEndPointAttributes()
        {
            var controllerType = typeof(ContactFormController);
            Assert.True(controllerType.GetMethod("SendMessage").GetCustomAttributes(typeof(EnableRateLimitingAttribute), true).Any());
            Assert.True(controllerType.GetMethod("GetTotal").GetCustomAttributes(typeof(AuthorizeAttribute), true).Any());
            Assert.True(controllerType.GetMethod("ListMessages").GetCustomAttributes(typeof(AuthorizeAttribute), true).Any());
            Assert.True(controllerType.GetMethod("GetMessage").GetCustomAttributes(typeof(AuthorizeAttribute), true).Any());
            Assert.True(controllerType.GetMethod("MarkAsRead").GetCustomAttributes(typeof(AuthorizeAttribute), true).Any());
        }
    }
}
