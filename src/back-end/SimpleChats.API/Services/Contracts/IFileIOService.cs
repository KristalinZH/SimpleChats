namespace SimpleChats.API.Services.Contracts
{
    public interface IFileIOService
    {
        Task<bool> ImportChatAsync(string json);
        Task<bool> ImportChatsAsync(string json);
        Task<string> ExportChatByIdAsync(Guid chatId);
        Task<string> ExportChatsAsync();
    }
}
