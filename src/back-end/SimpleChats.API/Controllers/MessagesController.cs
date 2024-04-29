namespace SimpleChats.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using ServiceModels;
    using Services.Contracts;

    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService messageService;
        private readonly IChatService chatService;
        public MessagesController(IMessageService _messageService, IChatService _chatService)
        {
            messageService = _messageService;
            chatService = _chatService;
        }
        [HttpGet]
        [Route("chat/{chatId}/all")]
        public async Task<IActionResult> AllMessages(string chatId)
        {
            try
            {
                bool isValidChatId = Guid.TryParse(chatId, out Guid cId);

                if (!isValidChatId)
                {
                    return BadRequest("Invalid type of chat id");
                }

                bool chatExists = await chatService.ChatExistsByIdAsync(cId);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                IEnumerable<MessageServiceModel> messages = await messageService.GetMessagesByChatIdAsync(cId);

                string serilizaedMessages = JsonConvert.SerializeObject(messages, Formatting.Indented);

                return Ok(serilizaedMessages);
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpPost]
        [Route("chat/sent")]
        public async Task<IActionResult> SentMessage([FromForm] MessageServiceModel message)
        {
            try
            {
                bool isValidChatId = Guid.TryParse(message.ChatId, out Guid cId);

                if (!isValidChatId)
                {
                    return BadRequest("Invalid type of chat id");
                }

                bool chatExists = await chatService.ChatExistsByIdAsync(cId);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await messageService.AddMessageAsync(message);

                return Ok("Message sent successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpPatch]
        [Route("chat/edit")]
        public async Task<IActionResult> EditMessage([FromForm] MessageServiceModel message)
        {
            try
            {
                bool isValidMessageId = Guid.TryParse(message.Id, out Guid mId);

                if (!isValidMessageId)
                {
                    return BadRequest("Invalid type of message id");
                }

                bool isValidChatId = Guid.TryParse(message.ChatId, out Guid cId);

                if (!isValidChatId)
                {
                    return BadRequest("Invalid type of chat id");
                }

                bool chatExists = await chatService.ChatExistsByIdAsync(cId);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                bool messageExists = await messageService.MessageExistByIdAsync(mId);

                if (!messageExists)
                {
                    return NotFound("Not existing message");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await messageService.UpdateMessageByIdAsync(mId, message.Text);

                return Ok("Message edited successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpDelete]
        [Route("chat/{chatId}")]
        public async Task<IActionResult> DeleteMessage(string chatId, [FromQuery] string messageId)
        {
            try
            {
                bool isValidChatId = Guid.TryParse(chatId, out Guid cId);

                if (!isValidChatId)
                {
                    return BadRequest("Invalid type of chat id");
                }

                bool isValidMessageId = Guid.TryParse(messageId, out Guid mId);

                if (!isValidMessageId)
                {
                    return BadRequest("Invalid type of message id");
                }

                bool chatExists = await chatService.ChatExistsByIdAsync(cId);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                bool messageExists = await messageService.MessageExistByIdAsync(mId);

                if (!messageExists)
                {
                    return NotFound("Not existing message");
                }

                await messageService.DeleteMessageByIdAsync(mId);

                return Ok("Message deleted successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
    }
}
