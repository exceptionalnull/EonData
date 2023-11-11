namespace EonData.ContactForm
{
    public interface IContactFormService
    {
        Task<int> GetTotalContactMessages(bool unreadOnly, CancellationToken cancellationToken);
        Task SaveContactMessageAsync(SendMessageModel message, string requestSource, CancellationToken cancellationToken);
    }
}