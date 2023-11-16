using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.ContactForm
{
    public class ContactMessage
    {
        public Guid MessageId { get; set; }
        public string? ContactName { get; set; }
        public string? ContactAddress { get; set; }
        public string? MessageContent { get; set; }
        public string? FormSource { get; set; }
        public string? RequestSource { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public bool isRead { get; set; }
    }
}
