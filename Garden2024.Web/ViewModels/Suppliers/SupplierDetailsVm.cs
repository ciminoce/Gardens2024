using System.ComponentModel;

namespace Garden2024.Web.ViewModels.Suppliers
{
    public class SupplierDetailsVm
    {
        public int SupplierId { get; set; }
        [DisplayName("Supplier Name")]

        public string SupplierName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string State { get; set; } = null!;

        public string City { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string? Phone { get; set; }

    }
}
