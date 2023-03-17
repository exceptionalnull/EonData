﻿using EonData.CloudControl.AWS;
using EonData.DomainEntities.ContactForm.Models;
using EonData.DomainLogic.ContactForm;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EonData.Api.Controllers
{
    [Route("contact")]
    [ApiController]
    public class ContactFormController : ControllerBase
    {
        private readonly S3FileStorageService _storage;
        private readonly ContactFormService _contactForm;

        public ContactFormController(S3FileStorageService storage)
        {
            _storage = storage;
            _contactForm = new ContactFormService(storage);
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Testing()
        {
            return Ok("TESTING 1 2 3!");
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SendMessage(ContactMessageModel message, CancellationToken cancellationToken)
        {   
            if (message == null)
            {
                return BadRequest("Message data is missing.");
            }

            try
            {
                await _contactForm.SaveContactMessageAsync(message, cancellationToken);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }

            return Ok();
        }
    }
}
