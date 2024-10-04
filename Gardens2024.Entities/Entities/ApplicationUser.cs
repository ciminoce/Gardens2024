using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Gardens2024.Entities.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        [Required]
        public string Address { get; set; } = null!;
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string ZipCode { get; set; } = null!;
        public string? Phone { get; set; }
        public Country Country { get; set; } = null!;
        public State State { get; set; } = null!;
        public City City { get; set; } = null!;

    }

}
