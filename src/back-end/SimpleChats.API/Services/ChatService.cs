namespace SimpleChats.API.Services
{
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    
    using Contracts;
    using ServiceModels;
    public class ChatService : IChatService
    {
        private readonly ChatDBContext context;
        public ChatService(ChatDBContext _context)
        {
            context = _context;
        }
        public async Task AddChatAsync(ChatServiceModel chat)
        {
            Chat newChat = new Chat()
            {
                ChatName = chat.ChatName,
                CreatedOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await context.Chats.AddAsync(newChat);
            await context.SaveChangesAsync();
        }
        public async Task<bool> ChatExistsByIdAsync(Guid id)
        {
            return await context.Chats.AnyAsync(c => c.Id == id && !c.IsDeleted);
        }
        public async Task DeleteChatByIdAsync(Guid id)
        {
            Chat? chat = await context.Chats.FindAsync(id);

            chat!.IsDeleted = true;

            await context.SaveChangesAsync();
        }
        public async Task EditChatNameAsync(Guid id, string newName)
        {
            Chat? chat = await context.Chats.FindAsync(id);

            chat!.ChatName = newName;

            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ChatServiceModel>> GetAllChats()
        {
            var data = await context.Chats
                .Where(c => !c.IsDeleted)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new
                {
                    Id = c.Id,
                    ChatName = c.ChatName,
                    CreatedOn = c.CreatedOn
                })
                .ToListAsync();

            IEnumerable<ChatServiceModel> chats = data.Select(c => new ChatServiceModel()
            {
                Id = c.Id.ToString(),
                ChatName = c.ChatName,
                CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
            });

            return chats;
        }
    }
}
