using System.ComponentModel.DataAnnotations;

namespace Mobify.Data
{
    public class Customers
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]                          
        [MaxLength(150)]                    
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(150)]
        public string Gender { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]                    
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]              
        public string PhoneNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]                         
        public string ConfirmPasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    }
}
