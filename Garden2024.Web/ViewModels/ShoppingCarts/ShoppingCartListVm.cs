using Gardens2024.Entities.Entities;

namespace Garden2024.Web.ViewModels.ShoppingCarts
{
    public class ShoppingCartListVm
    {
        public List<ShoppingCart>? ShoppingCarts { get; set; }
        public decimal OrderTotal { get; set; }
    }
}
