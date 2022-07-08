
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirportSystem.Data
{
    public class Airline
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(5)]
        [Remote("IsThisNameAlreadyExist", "Airlines", HttpMethod = "POST", ErrorMessage = "This airline already exist.")]
        public string Name { get; set; }
    }
}
