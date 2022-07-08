using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public record DeleteAndBookSeatModel(int Row, char Col, string SeatClass, int FlightID);
}
