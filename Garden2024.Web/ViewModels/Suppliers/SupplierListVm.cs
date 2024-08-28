using Gardens2024.Entities.Entities;

namespace Garden2024.Web.ViewModels.Suppliers
{
    public class SupplierListVm
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string? Phone { get; set; }
        public string Country { get; set; } = null!;
        public string State { get; set; } = null!;
        public string City { get; set; } = null!;

    }
}
