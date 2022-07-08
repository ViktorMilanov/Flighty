using AirportSystem.Enums;
using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Data
{
    public class Seat
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public int Row { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(6)]
        public char Col { get; set; }
        public bool Is_Booked { get; set; }
        [Required]
        public string Seat_Class { get; set; }
        public int Flight_id { get; set; }
        
    }
}
