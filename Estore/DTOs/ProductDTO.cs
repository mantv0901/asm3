namespace Estore.DTOs
{
    public class ProductDTO
    {
        public int ProductId { get; set; }  
        public string ProductName { get; set; }

        public decimal UnitPrice { get; set; }

        public int UnitsInStock { get; set; }
        public bool? Status { get; set; }
        public string Weight { get; set; }
        public string CategoryName { get; set; }
    }
}
