using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Products
{
    public class ProductListVm
    {
        public int ProductId { get; set; }
        [DisplayName("Product")]
        public string ProductName { get; set; } = null!;
        public string Category { get; set; } = null!;
        [DisplayName("Q. Unit ")]
        public string? QuantityPerUnit { get; set; }
        [DisplayName("Price")]
        public decimal UnitPrice { get; set; }
        public double Stock { get; set; }
        public bool Suspended { get; set; }


    }
}
