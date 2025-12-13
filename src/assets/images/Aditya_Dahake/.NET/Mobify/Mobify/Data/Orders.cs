using System.ComponentModel.DataAnnotations;

namespace Mobify.Data
{
    public class Orders
    {
        [Key]
        public int OrderId { get; set; }
     
        [Required, MaxLength(100)]
        public string ReceiverName { get; set; }
      
        [Required, MaxLength(15)]
        public string ReceiverPhoneNo { get; set; }

        [MaxLength(255)]
        public string? ShippingAddress { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public int CustomerId { get; set; }
        public  Customers? Customers  { get; set; }

        [Required]
        public int ProductId { get; set; }
        public Products? Products { get; set; }
    }
}
