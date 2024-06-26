﻿namespace SimpleChats.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using static ModelsConstants;
    public class Chat
    {
        public Chat()
        {
            Messages = new HashSet<Message>();
        }
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(NameMaxLength)]
        public string ChatName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public virtual IEnumerable<Message> Messages { get; set; }
        public bool IsDeleted { get; set; }
    }
}
