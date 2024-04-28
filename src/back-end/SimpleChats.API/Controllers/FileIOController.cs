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
            string tempFilePath = string.Empty;

            try
            {
                bool chatExists = await chatService.ChatExistsByIdAsync(chatId);

                if (!chatExists)
                {
                    return NotFound("Not existing chat");
                }

                string exportedChat = await fileService.ExportChatByIdAsync(chatId);

                tempFilePath = Path.GetTempFileName();

                System.IO.File.WriteAllText(tempFilePath, exportedChat);

                return File(tempFilePath, "application/json", "chat.json");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
            finally
            {
                System.IO.File.Delete(tempFilePath);
            }
        }
        [HttpGet]
        [Route("export/all")]
        public async Task<IActionResult> ExportAll()
        {
            string tempFilePath = string.Empty;

            try
            {
                string exportedChat = await fileService.ExportChatsAsync();

                tempFilePath = Path.GetTempFileName();

                System.IO.File.WriteAllText(tempFilePath, exportedChat);

                return File(tempFilePath, "application/json", "allChats.json");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
            finally
            {
                System.IO.File.Delete(tempFilePath);
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

                    await fileService.ImportChatAsync(content);
                }

                return RedirectToAction("AllChats", "Chats");
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

                    await fileService.ImportChatsAsync(content);
                }

                return RedirectToAction("AllChats", "Chats");
            }
            catch
            {
                return StatusCode(500, "Unexpected error occured!");
            }
        }
    }
}
