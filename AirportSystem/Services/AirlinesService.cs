using AirportDBFirst;
using AirportSystem.Data;
using AirportSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace AirportSystem.Services
{
    public class AirlinesService
    {
            private readonly AirportDbContext _context;

            public AirlinesService(AirportDbContext context)
            {
            
                _context = context;
            }
            public IEnumerable<Airline> GetAll()
            {
                return _context.Airline;
            }
            public void Create(Airline airline)
            {
                _context.Airline.Add(airline);
                _context.SaveChanges();
            }
            public void Edit(Airline airline)
            {
                var current = _context.Airline.FirstOrDefault(a => a.Id == airline.Id);
                current.Name = airline.Name;
                _context.Update(current);
                _context.SaveChanges();
            }
            public void Delete(int? id)
            {
                Airline airline = GetAirline((int)id);
                _context.Airline.Remove(airline);
                _context.SaveChanges();
            }
            public AirlineToDisplay GetForEdit(int id)
            {
                var airline = _context.Airline.FirstOrDefault(x => x.Id == id);
                if (airline == null)
                    return null;
                return new AirlineToDisplay {Name = airline.Name };
            }
            public Airline GetAirline(int id)
            {
                return _context.Airline.FirstOrDefault(x => x.Id == id);
            }

        public bool IsThisNameAlreadyExist(string airlineName)
        {
            return !_context.Airline.Any(a => a.Name == airlineName);
        }
    }
    }
