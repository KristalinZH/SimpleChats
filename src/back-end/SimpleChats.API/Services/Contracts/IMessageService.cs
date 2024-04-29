namespace SimpleChats.API.Services.Contracts
{
    using ServiceModels;

    public interface IMessageService
    {
        Task AddMessageAsync(MessageServiceModel message);
        Task UpdateMessageByIdAsync(Guid id, string newText);
        Task DeleteMessageByIdAsync(Guid id);
        Task<IEnumerable<MessageServiceModel>> GetMessagesByChatIdAsync(Guid chatId);
        Task<bool> MessageExistByIdAsync(Guid id);
    }
}
