using AirportSystem.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace AirportDBFirst
{
    public class AirportDbContext : DbContext
    {
        public DbSet<Airport> Airport { get; set; }
        public DbSet<Airline> Airline { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Seat> Seat { get; set; }



        public AirportDbContext(DbContextOptions<AirportDbContext> options)
        : base(options)
        {
        }

        internal int FirstOrDefault()
        {
            throw new NotImplementedException();
        }
    }
}