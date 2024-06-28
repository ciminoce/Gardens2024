namespace Gardens2024.Entities.Entities
{
    public class City
    {
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; } = null!;

    }
}
