using AirportDBFirst;
using AirportSystem.Data;
using AirportSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirportSystem.Services
{
    public class FlightsService
    {
        private readonly AirportDbContext _context;

        public FlightsService(AirportDbContext context)
        {
            _context = context;
        }
        public IEnumerable<FlightListModel> GetAll(List<Flight> flights)
        {
            return flights.Select(f => new FlightListModel(
_context.Airline.FirstOrDefault(a => a.Id == f.Airline_id).Name,
_context.Airport.FirstOrDefault(a => a.ID == f.Airport_origin_id).Port_Id,
_context.Airport.FirstOrDefault(a => a.ID == f.Airport_destination_id).Port_Id,
f.Take_Off_Date, f.Landing_Date, _context.Seat.Where(s => s.Flight_id == f.Id).Count(), _context.Seat.Where(s => s.Flight_id == f.Id && !s.Is_Booked).Count(), f.Id
));
        }

        public List<Flight> EditedFlights()
        {
            var flights = _context.Flight.ToList();
            foreach (var f in flights)
            {
                int count = 0;
                if(f.Max_Seat_Count < _context.Seat.Where(s => s.Flight_id == f.Id).Count())
                {
                    count = _context.Seat.Where(s => s.Flight_id == f.Id).Count() - f.Max_Seat_Count;
                    for (int i = 0; i < count; i++)
                    {
                        Seat s = new Seat { Id = 0 };
                        foreach (var j in _context.Seat.Where(s => s.Flight_id == f.Id))
                        {
                            if(j.Id > s.Id)
                            {
                                s = j;
                            }
                        }
                        _context.Seat.Remove(s);
                        _context.SaveChanges();
                    }
                }
            }
            return flights;
        }

        public void CreateFlight(FlightCreateModel flight)
        {
            int airlineId = _context.Airline.FirstOrDefault(f => f.Name == flight.AirlineName).Id;
            int airportOrgId = _context.Airport.FirstOrDefault(f => f.Port_Id == flight.OriginAirportId).ID;
            int airportDestId = _context.Airport.FirstOrDefault(f => f.Port_Id == flight.DestinationAirportId).ID;
            Flight flightToAdd = new Flight();
            flightToAdd.Landing_Date = flight.Landing_Date;
            flightToAdd.Take_Off_Date = flight.TakeOff_Date;
            flightToAdd.Rows = flight.Rows;
            flightToAdd.Cols = flight.Cols;
            flightToAdd.Airline_id = airlineId;
            flightToAdd.Airport_origin_id = airportOrgId;
            flightToAdd.Airport_destination_id = airportDestId;
            flightToAdd.Max_Seat_Count = flight.Rows * flight.Cols;
            _context.Flight.Add(flightToAdd);
            _context.SaveChanges();
        }



        public FlightDeleteModel GetForDelete(int id)
        {
            var flight = _context.Flight.FirstOrDefault(x => x.Id == id);
            if (flight == null)
                return null;
            return new FlightDeleteModel(
                _context.Airline.FirstOrDefault(a => a.Id == flight.Airline_id).Name,
                _context.Airport.FirstOrDefault(a => a.ID == flight.Airport_origin_id).Port_Id,
                _context.Airport.FirstOrDefault(a => a.ID == flight.Airport_destination_id).Port_Id,
                flight.Take_Off_Date, flight.Landing_Date, flight.Rows, flight.Cols);

        }

        public List<Flight> FindFlights(SearchModel modelForSearchingFlight)
        {
            int airportOrgId = _context.Airport.FirstOrDefault(f => f.Port_Id == modelForSearchingFlight.OriginAirportId).ID;
            int airportDestId = _context.Airport.FirstOrDefault(f => f.Port_Id == modelForSearchingFlight.DestinationAirportId).ID;
            List<Flight> potentialSearchedFlights = _context.Flight.Where(f => f.Airport_origin_id == airportOrgId && f.Airport_destination_id == airportDestId && modelForSearchingFlight.Date.CompareTo(f.Take_Off_Date) == 0).ToList();
            for (int i = 0; i < potentialSearchedFlights.Count(); i++)
            {
                Flight f = potentialSearchedFlights[i];
                List<Seat> seats = _context.Seat.Where(s => s.Flight_id == f.Id && !s.Is_Booked).ToList();
                if(seats.Count() < modelForSearchingFlight.CountOfPeopleWhoWillTakeTheFlight)
                {
                    potentialSearchedFlights.Remove(potentialSearchedFlights[i]);
                    i = -1;
                }
            }
            return potentialSearchedFlights;
        }

        public IEnumerable<FlightListModel> ToFlightListModel(List<Flight> flights)
        {
            return flights.Select(f => new FlightListModel(
                    _context.Airline.FirstOrDefault(a => a.Id == f.Airline_id).Name,
                    _context.Airport.FirstOrDefault(a => a.ID == f.Airport_origin_id).Port_Id,
                    _context.Airport.FirstOrDefault(a => a.ID == f.Airport_destination_id).Port_Id,
                    f.Take_Off_Date, f.Landing_Date, _context.Seat.Where(s => s.Flight_id == f.Id).Count(), _context.Seat.Where(s => s.Flight_id == f.Id && !s.Is_Booked).Count(), f.Id
                    ));
        }

        public void Delete(int? id)
        {
            Flight flight = _context.Flight.FirstOrDefault(x => x.Id == id);
            _context.Flight.Remove(flight);
            _context.SaveChanges();
        }

        public List<string> GetAllAirlinesName()
        {
            var airlines = _context.Airline;
            return airlines.Select(f => f.Name).ToList();
        }

        public List<string> GetAllAirportsId()
        {
            var airports = _context.Airport;
            return airports.Select(f => f.Port_Id).ToList();
        }
        public bool HasThisDateRassed(DateTime takeOff_Date)
        {
           int a = takeOff_Date.CompareTo(DateTime.Now);
            if(a < 0)
            {
                return false;
            }
            return true;
        }
    }
}
