using EonData.ContactForm.Models;

namespace EonData.ContactForm
{
    public interface IContactFormService
    {
        Task<int> GetTotalContactMessages(bool unreadOnly, CancellationToken cancellationToken);
        Task SaveContactMessageAsync(ContactMessageModel message, CancellationToken cancellationToken);
    }
}