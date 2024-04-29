namespace SimpleChats.API.Controllers
{
    using System.Text;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;

    [Route("api/[controller]")]
    [ApiController]
    public class FileIOController : ControllerBase
    {
        private readonly IFileIOService fileService;
        private readonly IChatService chatService;
        public FileIOController(IFileIOService _fileService, IChatService _chatService)
        {
            fileService = _fileService;
            chatService = _chatService;
        }
        [HttpGet]
        [Route("export/{chatId}")]
        public async Task<IActionResult> ExportChat(string chatId)
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

                string exportedChat = await fileService.ExportChatByIdAsync(id);

                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(exportedChat));

                return File(stream, "application/json", "chat.json");

            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpGet]
        [Route("export/all")]
        public async Task<IActionResult> ExportAll()
        {
            try
            {
                string exportedChats = await fileService.ExportChatsAsync();

                MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(exportedChats));

                return File(stream, "application/json", "allChats.json");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpPost]
        [Route("import/chat")]
        public async Task<IActionResult> ImportChat(IFormFile file)
        {
            try
            {
                bool isJsonFile = file.ContentType == "application/json"
                    || Path.GetExtension(file.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase)
                    || Path.GetExtension(file.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase);

                if (!isJsonFile || file.Length == 0)
                {
                    return BadRequest("Invalid file type");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    byte[] byteContent = stream.ToArray();

                    string content = Encoding.UTF8.GetString(byteContent);

                    bool success = await fileService.ImportChatAsync(content);

                    if (!success)
                    {
                        return BadRequest("Invalid file content type!");
                    }
                }

                return Ok("Chat imported successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
        [HttpPost]
        [Route("import/chats")]
        public async Task<IActionResult> ImportChats(IFormFile file)
        {
            try
            {
                bool isJsonFile = file.ContentType == "application/json"
                    || Path.GetExtension(file.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase)
                    || Path.GetExtension(file.FileName).Equals(".json", StringComparison.OrdinalIgnoreCase);

                if (!isJsonFile || file.Length == 0)
                {
                    return BadRequest("Invalid file type");
                }

                using (MemoryStream stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);

                    byte[] byteContent = stream.ToArray();

                    string content = Encoding.UTF8.GetString(byteContent);

                    bool success = await fileService.ImportChatsAsync(content);

                    if (!success)
                    {
                        return BadRequest("Invalid file content type!");
                    }
                }

                return Ok("Chats imported successfully!");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
    }
}
