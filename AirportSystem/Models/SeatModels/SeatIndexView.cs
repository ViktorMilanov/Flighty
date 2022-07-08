using AirportSystem.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public record SeatIndexView(int Row,char Col,string SeatClass, bool isBooked, int Id);
}
