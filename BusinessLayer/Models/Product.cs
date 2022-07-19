using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BusinessLayer.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(50)]
        public string ProductName { get; set; }
        [Required]
        [MaxLength(10)]
        public decimal UnitPrice { get; set; }
        [Required]
        [MaxLength(6)]
        public int UnitsInStock { get; set; }
        public bool? Status { get; set; }
        [Required]
        [MaxLength(20)]
        public string Weight { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
