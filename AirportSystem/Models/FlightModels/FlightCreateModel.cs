using AirportSystem.Enums;
using DataAnnotationsExtensions;
using Foolproof;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public class FlightCreateModel
    {
        public FlightCreateModel()
        {
            AirlinesNames = new List<string>();
            AirportsIds = new List<string>();
        }
        [Required]
        [Display(Name = "Airline Name")]
        public string AirlineName { get; set; }
        [Required]
        [Display(Name = "Origin Airport Id")]
        public string OriginAirportId { get; set; }
        [Display(Name = "Destination Airport Id")]
        [Required]
        public string DestinationAirportId { get; set; }
        [Required]
        [Remote("HasThisDateRassed", "Flights", HttpMethod = "POST", ErrorMessage = "The date has already passed")]
        [Display(Name = "Takeoff Date")]
        public DateTime TakeOff_Date { get; set; }
        [Required]
        [Display(Name = "Landing Date")]
        public DateTime Landing_Date { get; set; }
        [Required] [Max(20)] [Min(1)]
        public int Rows { get; set; }
        [Required]
        [Max(6)]
        [Min(1)]
        public int Cols { get; set; }
        public int Id { get; set; }
        public List<string> AirlinesNames { get; set; }
        public List<string> AirportsIds { get; set; }
        public int Error { get; set; }
    }

}

