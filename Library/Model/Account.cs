using System.ComponentModel.DataAnnotations;

namespace Library.Model
{
    public class Account
    {
        [Required]
        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string? FullName { get; set; }
        public int Age { get; set; }
        [Required]
        public string Role { get; set; } // e.g., "Admin", "Guest", "Customer"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
