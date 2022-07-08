using AirportSystem.Data;
using AirportSystem.Models;
using AirportSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirportSystem.Controllers
{
    public class SeatsController : Controller
    {
            private readonly SeatsService _service;

            public SeatsController(SeatsService service)
            {
                _service = service;
            }

            public IActionResult Index(int flightId)
            {
                return View(_service.GetAll(flightId));
            }
     
            public IActionResult Create(int flightId)
            {
                return View(new Create(_service.GetRows(flightId), _service.GetCols(flightId),flightId));
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(string flId)
            {
            for (int i = 0; i < Request.Form.Count / 3; i++)
            {
                _service.CreateSeat(new Seat { Row = int.Parse(Request.Form[$"flight[{i + 1}][row]"]), Col = char.Parse(Request.Form[$"flight[{i + 1}][col]"]), Seat_Class = Request.Form[$"flight[{i + 1}][class]"], Is_Booked = false, Flight_id = int.Parse(flId) });
            }
            return RedirectToAction("Index", "Flights");
            }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = _service.GetForDeleteOrBook((int)id);
            if (seat == null)
            {
                return NotFound();
            }   

            return View(seat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            int flId = _service.Delete(id);
            return View("Index", _service.GetAll(flId));
        }
        public IActionResult Book(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seat = _service.GetForDeleteOrBook((int)id);
            if (seat == null)
            {
                return NotFound();
            }

            return View(seat);
        }

        [HttpPost, ActionName("Book")]
        [ValidateAntiForgeryToken]
        public IActionResult BookConfirmed(int? id)
        {
            int flId = _service.Book(id);
            return View("Index", _service.GetAll(flId));
        }
    }
    }
