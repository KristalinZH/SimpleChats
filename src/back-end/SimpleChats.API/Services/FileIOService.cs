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
        public async Task<string> ExportChatByIdAsync(Guid chatId)
        {
            Chat? chat = await context.Chats.FindAsync(chatId);

            Message[] messages = await context.Messages
                .Where(m => m.ChatId == chatId && !m.IsDeleted)
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

            var anonChat = new
            {
                ChatName = chat!.ChatName,
                CreatedOn = chat!.CreatedOn.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture),
                Messages = anonMessages
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
        public async Task<bool> ImportChatAsync(string json)
        {
            try
            {
                var desChat = JsonConvert.DeserializeAnonymousType(json, new
                {
                    ChatName = "",
                    CreatedOn = "",
                    Messages = new[]
                    {
                        new
                        {
                            Text = "",
                            Sender = "",
                            Receiver = "",
                            SentOn = ""
                        }
                    }
                });

                Chat chat = new Chat()
                {
                    ChatName = desChat!.ChatName,
                    CreatedOn = DateTime.Parse(desChat!.CreatedOn, CultureInfo.InvariantCulture)
                };

                await context.Chats.AddAsync(chat);

                Message[] messages = desChat!.Messages
                    .Select(x => new Message()
                    {
                        ChatId = chat.Id,
                        Text = x.Text,
                        Sender = x.Sender,
                        Receiver = x.Receiver,
                        SentOn = DateTime.Parse(x.SentOn, CultureInfo.InvariantCulture)
                    })
                    .ToArray();

                await context.Messages.AddRangeAsync(messages);
                await context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }   
        }
        public async Task<bool> ImportChatsAsync(string json)
        {
            try
            {
                var desChats = JsonConvert.DeserializeAnonymousType(json, new[]
                {
                    new
                    {
                        ChatName = "",
                        CreatedOn = "",
                        Messages = new[]
                        {
                            new
                            {
                                Text = "",
                                Sender = "",
                                Receiver = "",
                                SentOn = ""
                            }
                        }
                    }
                });

                Chat[] chats = desChats!
                    .Select(c => new Chat()
                    {
                        ChatName = c.ChatName,
                        CreatedOn = DateTime.Parse(c.CreatedOn, CultureInfo.InvariantCulture)
                    })
                    .ToArray();

                await context.Chats.AddRangeAsync(chats);

                List<Message> messages = new List<Message>();
                Message[] chatMessages;

                for (int i = 0; i < chats.Length; i++)
                {
                    chatMessages = desChats![i].Messages
                        .Select(m => new Message()
                        {
                            ChatId = chats[i].Id,
                            Text = m.Text,
                            Sender = m.Sender,
                            Receiver = m.Receiver,
                            SentOn = DateTime.Parse(m.SentOn, CultureInfo.InvariantCulture)
                        })
                        .ToArray();

                    messages.AddRange(chatMessages);
                }

                await context.Messages.AddRangeAsync(messages);
                await context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
