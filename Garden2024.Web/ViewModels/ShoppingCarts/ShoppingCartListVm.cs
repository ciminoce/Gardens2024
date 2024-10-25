using Garden2024.Web.ViewModels.OrderHeaders;
using Gardens2024.Entities.Entities;

namespace Garden2024.Web.ViewModels.ShoppingCarts
{
    public class ShoppingCartListVm
    {
        public List<ShoppingCart>? ShoppingCarts { get; set; }
        public OrderHeaderEditVm? OrderHeader { get; set; }
    }
}
