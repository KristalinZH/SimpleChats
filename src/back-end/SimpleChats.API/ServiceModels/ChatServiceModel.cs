namespace SimpleChats.API.ServiceModels
{
    using System.ComponentModel.DataAnnotations;
    using static Data.Models.ModelsConstants;
    public class ChatServiceModel
    {
        public ChatServiceModel()
        {
            Messages = new HashSet<MessageServiceModel>();
        }
        public string Id { get; set; } = null!;
        [Required]
        [StringLength(NameMaxLength, MinimumLength = 3, ErrorMessage ="Invalid chat name")]
        public string ChatName { get; set; } = null!;
        public string? CreatedOn { get; set; }
        public IEnumerable<MessageServiceModel> Messages { get; set; }
    }
}
