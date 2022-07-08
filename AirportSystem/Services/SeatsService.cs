using AirportDBFirst;
using AirportSystem.Data;
using AirportSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Services
{
    public class SeatsService : Controller
    {
        private readonly AirportDbContext _context;

        public SeatsService(AirportDbContext context)
        {
            _context = context;
        }
        public IEnumerable<SeatIndexView> GetAll(int flightId)
        {
            var seats = _context.Seat.Where(s => s.Flight_id == flightId);
            return seats.Select(s => new SeatIndexView(s.Row, s.Col, s.Seat_Class.ToString(), s.Is_Booked, s.Id));
        } 
        public void CreateSeat(Seat seat)
        {
            var s = _context.Seat.FirstOrDefault(i => i.Flight_id == seat.Flight_id && i.Col == seat.Col && i.Row == seat.Row && i.Seat_Class == seat.Seat_Class);
            if (s == null)
            {
                _context.Seat.Add(seat);
                _context.SaveChanges();
            }
        }

        public int GetRows(int flightId)
        {
            return _context.Flight.First(f => f.Id == flightId).Rows;
        }

        public int GetCols(int flightId)
        {
            return _context.Flight.First(f => f.Id == flightId).Cols;
        }

        public DeleteAndBookSeatModel GetForDeleteOrBook(int id)
        {
            var seat = _context.Seat.FirstOrDefault(x => x.Id == id);
            if (seat == null)
                return null;
            return new DeleteAndBookSeatModel(seat.Row, seat.Col, seat.Seat_Class, seat.Flight_id);
        }

        public int Delete(int? id)
        {
            Seat seat = _context.Seat.FirstOrDefault(x => x.Id == id);
            int flID = seat.Flight_id;
            _context.Seat.Remove(seat);
            _context.SaveChanges();
            return flID;
        }
        public int Book(int? id)
        {
            Seat seat = _context.Seat.FirstOrDefault(x => x.Id == id);
            seat.Is_Booked = true;
            int flID = seat.Flight_id;
            _context.Seat.Update(seat);
            _context.SaveChanges();
            return flID;
        }
    }
}
