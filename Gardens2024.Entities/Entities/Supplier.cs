namespace Gardens2024.Entities.Entities
{
    public class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string ZipCode { get; set; } = null!;
        public string? Phone { get; set; }
        public Country Country { get; set; } = null!;
        public State State { get; set; } = null!;
        public City City { get; set; } = null!;
    }
}
