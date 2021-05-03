using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Date
    {
        [Key] public Guid Id { get; set; }

        [Required] public string UserId { get; set; }
        
        public User User { get; set; }

        [Required] public string OtherId { get; set; }
        
        public User Other { get; set; }
        
        [Required]
        public DateTime Timestamp { get; set; }
        
        public void Swap(User currentUser)
        {
            if (User.Id == currentUser.Id)
            {
                return;
            }

            Other = User;
            User = currentUser;
        }
    }

    public class DateResponse
    {
        public Guid Id { get; set; }

        public UserResponse Other { get; set; }
    }
}