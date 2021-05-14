using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ChatMessageRequest
    {
        [Required] public Guid DateId { get; set; }
        
        [Required] [StringLength(512)] public string Message { get; set; }
    }

    public class ChatMessage
    {
        [Required] public Guid DateId { get; set; }

        [Required] [StringLength(512)] public string Message { get; set; }
        
        [Required] public DateTime Timestamp { get; set; }

        public ChatMessage()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}