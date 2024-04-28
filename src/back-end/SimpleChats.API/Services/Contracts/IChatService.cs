namespace SimpleChats.API.Services.Contracts
{
    using ServiceModels;

    public interface IChatService
    {
        Task AddChatAsync(ChatServiceModel chat);
        Task DeleteChatByIdAsync(string id);
        Task EditChatNameAsync(string id, string newName);
        Task<bool> ChatExistsByIdAsync(string id);
        Task<IEnumerable<ChatServiceModel>> GetAllChats();
    }
}
