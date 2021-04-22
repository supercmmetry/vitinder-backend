using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Hate
    {
        [Key] public Guid Id { get; set; }
        
        [StringLength(64)] [Required] public string Name { get; set; }
        
        public ICollection<User> Users { get; set; }
    }

    public class HateCreate
    {
        [StringLength(64)] [Required] public string Name { get; set; }
    }
    
    public class HateUpdate
    {
        [Required] public Guid Id { get; set; }
        
        [StringLength(64)] [Required] public string Name { get; set; }
    }

    public class HateRequest
    {
        [Required] public Guid Id { get; set; }
    }

    public class HateResponse
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }
}