using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AirportSystem.Models
{
    public class AirportWithoutId
    {
        [Required]
        [MaxLength(3)]
        [MinLength(3)]
        [Display(Name = "ID")]
        [Remote("IsThisIdAlreadyExist", "Airports", HttpMethod = "POST", ErrorMessage = "Airport with that ID already exist.")]
        public string Port_Id { get; set; }
        [Required]
        [Remote("IsThisNameAlreadyExist", "Airports", HttpMethod = "POST", ErrorMessage = "Airport with that name already exist.")]
        public string Name { get; set; }
        [Required]
        [Remote("IsThisAdressAlreadyExist", "Airports", HttpMethod = "POST", ErrorMessage = "Airport with that adress already exist.")]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }    
    }
}