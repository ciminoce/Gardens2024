namespace Gardens2024.Entities.Entities
{
    public class ShoppingCart
    {
        //TODO: Migrar o crear tabla a mano
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public Product Product { get; set; } = null!;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
