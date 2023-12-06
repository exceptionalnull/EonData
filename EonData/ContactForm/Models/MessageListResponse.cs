using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EonData.ContactForm.Models
{
    /// <summary>
    /// This represents a list response of contact messages.
    /// </summary>
    /// <remarks>This is severly limited in abilities. It is possible to add things like more convenient pagination and sorting but it is probably better to replace this with a custom S3 backend. (See #23)</remarks>
    public class MessageListResponse
    {
        /// <summary>
        /// Messages on this page.
        /// </summary>
        public IEnumerable<MessageListModel> Messages { get; }

        /// <summary>
        /// The start value to be used for the next page's request
        /// </summary>
        public Guid? StartKey { get; set; }

        internal MessageListResponse(IEnumerable<MessageListModel> results, string? lastEvalutedKey)
        {
            Messages = results;
            StartKey = (lastEvalutedKey != null) ? new Guid(lastEvalutedKey) : null;
        }
    }
}
