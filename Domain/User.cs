using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class User
    {
        [Key] public string Id { get; set; }

        [StringLength(256)] [Required] public string FirstName { get; set; }

        [StringLength(256)] [Required] public string LastName { get; set; }
        
        [StringLength(8)] [Required] public string Sex { get; set; }
        
        [StringLength(32)] [Required] public string SexualOrientation { get; set; }

        [Range(16, 100)] [Required] public int Age { get; set; }
        
        [StringLength(64)] public string FieldOfStudy { get; set; }

        [Range(1, 16)] public int YearOfStudy { get; set; }
        
        [StringLength(512)] public string Bio { get; set; }
        
        [Range(0, 10)] [Required] public int AccessLevel { get; set; }

        public ICollection<Passion> Passions { get; set; }
        
        public ICollection<Hate> Hates { get; set; }
    }

    public class UserRequest
    {
        [StringLength(256)] [Required] public string FirstName { get; set; }

        [StringLength(256)] [Required] public string LastName { get; set; }

        [Range(16, 100)] [Required] public int Age { get; set; }
        
        [StringLength(8)] [Required] public string Sex { get; set; }
        
        [StringLength(32)] [Required] public string SexualOrientation { get; set; }
    }

    public class UserResponse
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        
        public int Age { get; set; }
        
        public string Sex { get; set; }
        
        public string SexualOrientation { get; set; }
    }
}