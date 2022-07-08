using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Models
{
    public record FlightDeleteModel( string AirlineName, string OriginAirportId, string DestinationAirportId, DateTime TakeOff_Date,  DateTime Landing_Date, int Rows, int Cols);
}
