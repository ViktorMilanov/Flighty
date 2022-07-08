using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirportSystem.Data
{
    public class Airport
    {
        public Airport()
        {
            Flights = new HashSet<Flight>();
        }
        [Key]
        public int ID { get; set; }
        [Remote("IsThisIdAlreadyExist", "Airports", HttpMethod = "POST", ErrorMessage = "Airport with that ID already exist.")]

        public string Port_Id { get; set; }
        [Remote("IsThisNameAlreadyExist", "Airports", HttpMethod = "POST", ErrorMessage = "Airport with that name already exist.")]
        public string Name { get; set; }
        [Remote("IsThisAdressAlreadyExist", "Airports", HttpMethod = "POST", ErrorMessage = "Airport with that adress already exist.")]
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<Flight> Flights { get; set; }
    }
}