using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Passion
    {
        [Key] public Guid Id { get; set; }
        
        [StringLength(64)] [Required] public string Name { get; set; }
        
        public ICollection<User> Users { get; set; }
    }

    public class PassionCreate
    {
        [StringLength(64)] [Required] public string Name { get; set; }
    }
    
    public class PassionUpdate
    {
        [Required] public Guid Id { get; set; }
        
        [StringLength(64)] [Required] public string Name { get; set; }
    }

    public class PassionRequest
    {
        [Required] public Guid Id { get; set; }
    }

    public class PassionResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }
}