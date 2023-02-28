using EonData.CloudControl.AWS;
using EonData.DomainEntities.ContactForm.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.DomainLogic.ContactForm
{
    internal class ContactFormService
    {
        private readonly S3FileStorageService _storage;
        public ContactFormService(S3FileStorageService storageService)
        {
            _storage = storageService;
        }

        public async Task SaveContactMessageAsync(ContactMessageModel message)
        {
            string filename = "contact-message-.json";
            _storage.WriteFile("eondata", $"contacts/{filename}", 
        }
    }
}
