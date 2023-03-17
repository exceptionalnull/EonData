using EonData.CloudControl.AWS;
using EonData.DomainEntities.ContactForm.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EonData.DomainLogic.ContactForm
{
    public class ContactFormService
    {
        private readonly S3FileStorageService _storage;
        public ContactFormService(S3FileStorageService storageService)
        {
            _storage = storageService;
        }

        public Task SaveContactMessageAsync(ContactMessageModel message, CancellationToken cancellationToken) => _storage.SaveFileAsync("eondataweb-data", $"contacts/contact-message-{DateTime.UtcNow:ddMMyyHHmmssfff}.json", JsonSerializer.Serialize(message), cancellationToken);
    }
}
