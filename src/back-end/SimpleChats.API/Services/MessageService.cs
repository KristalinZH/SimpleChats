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
    public class MessageService : IMessageService
    {
        private readonly ChatDBContext context;
        public MessageService(ChatDBContext _context)
        {
            context = _context;
        }
        public async Task AddMessageAsync(MessageServiceModel message)
        {
            Message newMessage = new Message()
            {
                Text = message.Text,
                Sender = message.Sender,
                Receiver = message.Receiver,
                ChatId = Guid.Parse(message.ChatId),
                SentOn = DateTime.UtcNow,
                IsDeleted = false
            };

            await context.Messages.AddAsync(newMessage);
            await context.SaveChangesAsync();
        }
        public async Task DeleteMessageByIdAsync(Guid id)
        {
            Message? message = await context.Messages.FindAsync(id);

            message!.IsDeleted = true;

            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<MessageServiceModel>> GetMessagesByChatIdAsync(Guid chatId)
        {
            var data = await context.Messages
                .Where(m => m.ChatId == chatId && !m.IsDeleted)
                .OrderByDescending(m => m.SentOn)
                .Select(m => new
                {
                    Id = m.Id,
                    Text = m.Text,
                    Sender = m.Sender,
                    Receiver = m.Receiver,
                    SentOn = m.SentOn
                })
                .ToListAsync();

            IEnumerable<MessageServiceModel> messages = data.Select(m => new MessageServiceModel()
            {
                Id = m.Id.ToString(),
                Text = m.Text,
                Sender = m.Sender,
                Receiver = m.Receiver,
                SentOn = m.SentOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture)
            });

            return messages;
        }
        public async Task UpdateMessageByIdAsync(Guid id, string newText)
        {
            Message? message = await context.Messages.FindAsync(id);

            message!.Text = newText;

            await context.SaveChangesAsync();
        }
        public async Task<bool> MessageExistByIdAsync(Guid id)
        {
            return await context.Messages.AnyAsync(m => m.Id == id && !m.IsDeleted);
        }
    }
}
