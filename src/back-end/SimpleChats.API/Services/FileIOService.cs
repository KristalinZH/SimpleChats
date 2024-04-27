namespace SimpleChats.API.Services
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;

    using Contracts;

    public class FileIOService : IFileIOService
    {
        private readonly ChatDBContext context;
        public FileIOService(ChatDBContext _context)
        {
            context = _context;
        }
        public async Task<string> ExportChatByIdAsync(string chatId)
        {
            Guid id = Guid.Parse(chatId);

            Chat? chat = await context.Chats.FindAsync(id);

            Message[] messages = await context.Messages
                .Where(m => m.ChatId == id && !m.IsDeleted)
                .ToArrayAsync();

            var anonMessages = messages
                .Select(m => new
                {
                    Text = m.Text,
                    Sender = m.Sender,
                    Receiver = m.Receiver,
                    SentOn = m.SentOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
                })
                .ToArray();
            
            string messagesSeriliazation = JsonConvert.SerializeObject(messages, Formatting.Indented);

            var anonChat = new
            {
                ChatName = chat!.ChatName,
                CreatedOn = chat!.CreatedOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture),
                Messages = messagesSeriliazation
            };

            return JsonConvert.SerializeObject(anonChat, Formatting.Indented);
        }

        public async Task<string> ExportChatsAsync()
        {
            Chat[] chats = await context.Chats
                .Where(c => !c.IsDeleted)
                .Include(c => c.Messages)
                .ToArrayAsync();

            var anonChats = chats.Select(c => new
            {
                ChatName = c.ChatName,
                CreatedOn = c.CreatedOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture),
                Messages = c.Messages.Select(m => new
                {
                    Text = m.Text,
                    Sender = m.Sender,
                    Receiver = m.Receiver,
                    SentOn = m.SentOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
                }).ToArray()
            }).ToArray();

            return JsonConvert.SerializeObject(anonChats, Formatting.Indented);
        }

        public Task ImportChatAsync()
        {
            throw new NotImplementedException();
        }

        public Task ImportChatsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
