using System.ComponentModel.DataAnnotations;

namespace AirportSystem.Models
{
    public record AirportToDisplay([Required][MaxLength(3)][MinLength(3)] string Port_id, [Required] int Id, [Required] string name, [Required] string Address, [Required] string city, [Required] string country);
}