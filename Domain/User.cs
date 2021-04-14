using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key] public string Id { get; set; }

        [Required] public string FirstName { get; set; }

        [Required] public string LastName { get; set; }
        
        [Required] public int AccessLevel { get; set; }
    }

    public class UserRequest
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }

    }

    public class UserResponse
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; } 
    }
}