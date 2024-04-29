namespace SimpleChats.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using ServiceModels;
    using Services.Contracts;
    using SimpleChats.Data.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IChatService chatService;
        public ChatsController(IChatService _chatService)
        {
            chatService = _chatService;
        }
        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> AllChats()
        {
            try
            {
                IEnumerable<ChatServiceModel> chats = await chatService.GetAllChats();

                string seriliazedChats = JsonConvert.SerializeObject(chats, Formatting.Indented);

                return Ok(seriliazedChats);
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateChat([FromForm] ChatServiceModel chat)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await chatService.AddChatAsync(chat);

                return Ok("Chat created succesfully");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpPatch]
        [Route("rename")]
        public async Task<IActionResult> RenameChat([FromForm] ChatServiceModel chat)
        {
            try
            {
                bool isValidId = Guid.TryParse(chat.Id, out Guid id);

                if (!isValidId)
                {
                    return BadRequest("Invalid type of id");
                }

                bool chatExists = await chatService.ChatExistsByIdAsync(id);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await chatService.EditChatNameAsync(id, chat.ChatName);

                return Ok("Chat renamed successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpDelete]
        [Route("delete/{chatId}")]
        public async Task<IActionResult> DeleteChat(string chatId)
        {
            try
            {
                bool isValidId = Guid.TryParse(chatId, out Guid id);

                if (!isValidId)
                {
                    return BadRequest("Invalid type of id");
                }

                bool chatExists = await chatService.ChatExistsByIdAsync(id);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                await chatService.DeleteChatByIdAsync(id);

                return Ok("Chat deleted successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
    }
}
