using Garden2024.Web.ViewModels.Products;

namespace Garden2024.Web.ViewModels.ShoppingCarts
{
    public class ShoppingCartDetailVm
    {
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public ProductHomeDetailsVm Product { get; set; } = null!;

    }
}
