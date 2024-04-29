namespace SimpleChats.API.Services.Contracts
{
    using ServiceModels;

    public interface IChatService
    {
        Task AddChatAsync(ChatServiceModel chat);
        Task DeleteChatByIdAsync(Guid id);
        Task EditChatNameAsync(Guid id, string newName);
        Task<bool> ChatExistsByIdAsync(Guid id);
        Task<IEnumerable<ChatServiceModel>> GetAllChats();
    }
}
