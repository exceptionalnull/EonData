using System.Numerics;
using EonData.ContactForm.Models;

namespace EonData.ContactForm.Services
{
    public interface IContactFormService
    {
        Task<int> GetTotalContactMessagesAsync(bool? unread, CancellationToken cancellationToken);
        Task SaveContactMessageAsync(SendMessageModel message, string requestSource, CancellationToken cancellationToken);
        Task<IEnumerable<MessageListModel>> ListMessagesAsync(bool? unread, CancellationToken cancellationToken);
        Task<ContactMessageModel?> GetContactMessageAsync(Guid messageId, CancellationToken cancellationToken);
        Task MarkAsReadAsync(Guid messageId, CancellationToken cancellationToken);
    }
}