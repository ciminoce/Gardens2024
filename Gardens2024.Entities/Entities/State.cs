namespace Gardens2024.Entities.Entities
{
    public class State
    {
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; } = null!;
        public Country Country { get; set; } = null!;
    }
}
