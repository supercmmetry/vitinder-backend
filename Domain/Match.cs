using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Match
    {
        [Key] public Guid Id { get; set; }

        [Required] public string UserId { get; set; }
        public User User { get; set; }

        [Required] public string OtherId { get; set; }
        public User Other { get; set; }

        [Required] public bool Status { get; set; }
    }
    
    public class MatchRequest
    {
        [Required] public string UserId { get; set; }
        
        [Required] public string OtherId { get; set; }

        [Required] public bool Status { get; set; }
    }
    
    public class MatchResponse
    {
        public string OtherId { get; set; }
        
        public UserResponse Other { get; set; }

        public bool Status { get; set; }
    }
}