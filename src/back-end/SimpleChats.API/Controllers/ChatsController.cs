namespace SimpleChats.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    using ServiceModels;
    using Services.Contracts;

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
                bool chatExists = await chatService.ChatExistsByIdAsync(chat.Id);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await chatService.EditChatNameAsync(chat.Id, chat.ChatName);

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
                bool chatExists = await chatService.ChatExistsByIdAsync(chatId);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                await chatService.DeleteChatByIdAsync(chatId);

                return Ok("Chat deleted successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
    }
}
