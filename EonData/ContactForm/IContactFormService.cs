using System.Numerics;

namespace EonData.ContactForm
{
    public interface IContactFormService
    {
        Task<int> GetTotalContactMessagesAsync(bool unreadOnly, CancellationToken cancellationToken);
        Task SaveContactMessageAsync(SendMessageModel message, string requestSource, CancellationToken cancellationToken);
        Task<IEnumerable<MessageListModel>> ListMessagesAsync(bool unreadOnly, CancellationToken cancellationToken);
        Task<ContactMessage?> GetContactMessageAsync(Guid messageId, CancellationToken cancellationToken);
        Task MarkAsReadAsync(Guid messageId, CancellationToken cancellationToken);
    }
}