using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Match
    {
        [Key] public Guid Id { get; set; }

        public User User { get; set; }

        public User Other { get; set; }

        [Required] public bool Status { get; set; }
    }
    
    public class MatchRequest
    {
        [Required] public string UserId { get; set; }
        
        [Required] public string OtherId { get; set; }

        [Required] public bool Status { get; set; }
    }
}