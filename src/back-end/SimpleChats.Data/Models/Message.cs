namespace SimpleChats.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using static ModelsConstants;
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;
        [Required]
        [MaxLength(NameMaxLength)]
        public string Sender { get; set; } = null!;
        [Required]
        [MaxLength(NameMaxLength)]
        public string Receiver { get; set; } = null!;
        public DateTime SentOn { get; set; }
        public Guid ChatId { get; set; }
        [ForeignKey(nameof(ChatId))]
        public virtual Chat Chat { get; set; } = null!;
    }
}