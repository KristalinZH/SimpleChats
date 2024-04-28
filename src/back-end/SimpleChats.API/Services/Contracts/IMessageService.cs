namespace SimpleChats.API.Services.Contracts
{
    using ServiceModels;

    public interface IMessageService
    {
        Task AddMessageAsync(MessageServiceModel message);
        Task UpdateMessageByIdAsync(string id, string newText);
        Task DeleteMessageByIdAsync(string id);
        Task<IEnumerable<MessageServiceModel>> GetMessagesByChatIdAsync(string chatId);
        Task<bool> MessageExistByIdAsync(string id);
    }
}
