using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobify.Data
{
    public class Sellers
    {
        [Key]
        public int SellerId { get; set; }

        [Required]                          
        [MaxLength(150)]                     
        public string SellerName { get; set; }

        [Required]
        [MaxLength(150)]
        public string StoreName { get; set; }

        [Required]
        [MaxLength(150)]
        [EmailAddress]                   
        public string Email { get; set; }

        [Required]
        [MaxLength(15)]                     
        public string PhoneNo { get; set; }  

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]                  
        public string ConfirmPasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public virtual ICollection<Products> Products { get; set; } = new List<Products>();

        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    }
}
