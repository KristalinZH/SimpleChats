namespace SimpleChats.API.ServiceModels
{
    using System.ComponentModel.DataAnnotations;
    using static Data.Models.ModelsConstants;
    public class MessageServiceModel
    {
        public string Id { get; set; } = null!;
        [Required]
        [StringLength(TextMaxLength, MinimumLength = 1, ErrorMessage = "Too many or less characters" )]
        public string Text { get; set; } = null!;
        [Required]
        [StringLength(NameMaxLength, MinimumLength = 3, ErrorMessage = "Invalid name length")]
        public string Sender { get; set; } = null!;
        [Required]
        [StringLength(NameMaxLength, MinimumLength = 3, ErrorMessage = "Invalid name length")]
        public string Receiver { get; set; } = null!;
        [Required]
        public string ChatId { get; set; } = null!;
        public string? SentOn { get; set; }
    }
}
