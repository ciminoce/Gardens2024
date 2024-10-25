namespace Gardens2024.Entities.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? LatinName { get; set; }
        public int SupplierId { get; set; }
        public int CategoryId { get; set; }
        public string? QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public double Stock { get; set; }
        public double StockInCarts { get; set; }
        public double AvailableStock { get => Stock - StockInCarts; }
        public string? ImageUrl { get; set; }
        public bool Suspended { get; set; }
        public Category Category { get; set; } = null!;
        public Supplier Supplier { get; set; } = null!;
    }
}
