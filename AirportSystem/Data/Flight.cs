using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Data
{
    public class Flight
    {
        [Key]
        public int Id { get; set; }
        public DateTime Take_Off_Date { get; set; }
        public DateTime Landing_Date { get; set; }
        [Max(20)]
        [Min(1)]
        public int Rows { get; set; }
        [Max(6)]
        [Min(1)]
        public int Cols { get; set; }
        public int Airline_id { get; set; }
        public int Airport_origin_id { get; set; }
        public int Airport_destination_id { get; set; }
        public int Max_Seat_Count { get; set; }
    }
}
