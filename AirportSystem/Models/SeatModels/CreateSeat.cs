using AirportSystem.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public record CreateSeat([Required] int Row, [Required] char Col, [Required] string SeatClass, int FlightId);
}
