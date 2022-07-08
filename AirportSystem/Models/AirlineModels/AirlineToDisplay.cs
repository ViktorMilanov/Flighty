using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AirportSystem.Models
{
    public class AirlineToDisplay{
        [Required]
        [MaxLength(5)]
        [Remote("IsThisNameAlreadyExist", "Airlines", HttpMethod = "POST", ErrorMessage = "This airline already exist.")]
        public string Name { get; set; }
    }
}

