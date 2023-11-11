using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.ContactForm
{
    internal class MessageListModel
    {
        public Guid MessageId { get; set; }
        public DateTime MessageTimestamp { get; set; }
        public string ContactName { get; set; }
        public string ContactAddress { get; set; }
        public bool isRead { get; set; }
    }
}
