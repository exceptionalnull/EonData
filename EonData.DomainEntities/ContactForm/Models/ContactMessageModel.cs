using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.DomainEntities.ContactForm.Models
{
    public class ContactMessageModel
    {
        public string? ContactAddress { get; set; }
        public string? Message { get; set; }
        public string? Source { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
