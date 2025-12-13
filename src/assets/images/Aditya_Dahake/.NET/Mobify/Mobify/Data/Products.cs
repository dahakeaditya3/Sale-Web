using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mobify.Data
{
    public class Products
    {
            [Key]
            public int ProductId { get; set; }

            [Required]
            [MaxLength(150)]
            public string ProductName { get; set; }

            [Required]
            [MaxLength(100)]
            public string? ProductCompany { get; set; }


        [Required]
        [MaxLength(255)]
        public string? Description { get; set; }   

            [Required]
            [Column(TypeName = "decimal(10,2)")]
            public decimal Price { get; set; }

            [Required]
            public int Quantity { get; set; }

            [MaxLength(255)]
            public string? ImageUrl1 { get; set; }

            [MaxLength(255)]
            public string? ImageUrl2 { get; set; }

            [MaxLength(255)]
            public string? ImageUrl3 { get; set; }

            [MaxLength(255)]
            public string? ImageUrl4 { get; set; }

        [Required]
        [ForeignKey("Seller")]
        public int SellerId { get; set; }

        public virtual Sellers? Sellers { get; set; }

        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    }
}
