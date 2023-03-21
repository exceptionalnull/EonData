using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.DomainEntities.ContactForm.Models
{
    internal class ContactMessageViewModel
    {
        public string? Name { get; set; }
        public string? ContactAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string Filename { get; set; }
        public bool IsDeleted { get; set; }
    }
}