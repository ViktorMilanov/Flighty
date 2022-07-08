using AirportDBFirst;
using AirportSystem.Data;
using AirportSystem.Models;
using System.Collections.Generic;
using System.Linq;

namespace AirportSystem.Services
{
    public class AirportsService
    {
        private readonly AirportDbContext _context;

        public AirportsService(AirportDbContext context)
        {
            _context = context;
        }
        public IEnumerable<AirportToDisplay> GetAll()
        {
            return _context.Airport.Select(f => new AirportToDisplay(f.Port_Id, f.ID, f.Name, f.Address, f.City, f.Country));
        }
        public void Create(Airport airport)
        {
            _context.Airport.Add(airport);
            _context.SaveChanges();    
        }
        public void Edit(Airport airport)
        {
            var current = _context.Airport.FirstOrDefault(a => a.ID == airport.ID);
            current.Name = airport.Name;
            current.Address = airport.Address;
            current.City = airport.City;
            current.Country = airport.Country;
            _context.Update(current);
            _context.SaveChanges();
        }
        public void Delete(int? id)
        {
            Airport airport = GetAirport((int)id);
            _context.Airport.Remove(airport);
            _context.SaveChanges();
        }
         public AirportWithoutId GetForEdit(int id)
         {
             var airport = _context.Airport.FirstOrDefault(x => x.ID == id);
             if (airport == null)
                 return null;
             return new AirportWithoutId {Port_Id = airport.Port_Id,Name = airport.Name,Address = airport.Address,City = airport.City,Country = airport.Country };
         }
        public Airport GetAirport(int id)
        {
            return _context.Airport.FirstOrDefault(x => x.ID == id);
        }
        public bool IsThisNameAlreadyExist(string Name)
        {
            return !_context.Airport.Any(a => a.Name == Name);
        }
        public bool IsThisIdAlreadyExist(string Id)
        {
            return !_context.Airport.Any(a => a.Port_Id == Id);
        }
        public bool IsThisAdressAlreadyExist(string Adress)
        {
            return !_context.Airport.Any(a => a.Address == Adress);
        }
    }
}