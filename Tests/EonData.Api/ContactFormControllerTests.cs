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
