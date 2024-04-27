namespace SimpleChats.API.Services.Contracts
{
    public interface IFileIOService
    {
        Task ImportChatAsync(string json);
        Task ImportChatsAsync(string json);
        Task<string> ExportChatByIdAsync(string chatId);
        Task<string> ExportChatsAsync();
    }
}
