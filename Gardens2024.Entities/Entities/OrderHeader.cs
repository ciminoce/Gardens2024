namespace Gardens2024.Entities.Entities
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string ApplicationUserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public decimal OrderTotal { get; set; }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ApplicationUser? ApplicationUser { get; set; }
        public Country? Country { get; set; }
        public State? State { get; set; }
        public City? City { get; set; }
    }
}
