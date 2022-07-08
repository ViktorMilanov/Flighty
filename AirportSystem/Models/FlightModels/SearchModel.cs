using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public class SearchModel
    {
        [Required]
        public string OriginAirportId { get; set; }
        [Required]
        public string DestinationAirportId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int CountOfPeopleWhoWillTakeTheFlight { get; set; }
        [Required]
        public List<string> AirlinesNames { get; set; }
        [Required]
        public List<string> AirportsIds { get; set; }
        public int Error { get; set; }
    }
}
