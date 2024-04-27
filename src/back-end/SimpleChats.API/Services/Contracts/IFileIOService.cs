namespace SimpleChats.API.Services.Contracts
{
    public interface IFileIOService
    {
        Task ImportChatAsync();
        Task ImportChatsAsync();
        Task<string> ExportChatByIdAsync(string chatId);
        Task<string> ExportChatsAsync();
    }
}
