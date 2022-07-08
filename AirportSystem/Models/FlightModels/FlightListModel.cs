using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public record FlightListModel([Required] string AirlineName, [Required] string OriginAirportId, [Required] string DestinationAirportId, [Required] DateTime TakeOff_Date, [Required] DateTime Landing_Date, [Required] int AllSeats, [Required] int FreeSeats, int Id);
}
