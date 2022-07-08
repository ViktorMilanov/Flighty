
using AirportSystem.Models;
using AirportSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace AirportSystem.Controllers
{
    public class FlightsController : Controller
    {
        private readonly FlightsService _service;

        public FlightsController(FlightsService service)
        {
            _service = service;
        }
        public IActionResult Index()
        {
            return View(_service.GetAll(_service.EditedFlights()));
        }
        public IActionResult Create()
        {
            return View(new FlightCreateModel {TakeOff_Date = DateTime.Today, Landing_Date = DateTime.Today, AirlinesNames = _service.GetAllAirlinesName(), AirportsIds = _service.GetAllAirportsId() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(FlightCreateModel flight)
        {
            int resultFromCompareTwoDates = flight.TakeOff_Date.CompareTo(flight.Landing_Date);
            if(resultFromCompareTwoDates >= 0)
            {
                return View(new FlightCreateModel {Error = 1,Rows = flight.Rows, Cols = flight.Cols, AirlineName = flight.AirlineName, DestinationAirportId = flight.DestinationAirportId, OriginAirportId = flight.OriginAirportId, TakeOff_Date = DateTime.Today, Landing_Date = DateTime.Now, AirlinesNames = _service.GetAllAirlinesName(), AirportsIds = _service.GetAllAirportsId() });
            }
            if (flight.OriginAirportId == flight.DestinationAirportId)
            {
                return View(new FlightCreateModel { Error = 2, Rows = flight.Rows, Cols = flight.Cols, AirlineName = flight.AirlineName, DestinationAirportId = flight.DestinationAirportId, OriginAirportId = flight.OriginAirportId, TakeOff_Date = DateTime.Today, Landing_Date = DateTime.Now, AirlinesNames = _service.GetAllAirlinesName(), AirportsIds = _service.GetAllAirportsId() });
            }
            if (ModelState.IsValid)
            {
                _service.CreateFlight(flight);
                return RedirectToAction(nameof(Index));
            }
            return View(new FlightCreateModel { TakeOff_Date = DateTime.Today, Landing_Date = DateTime.Now, AirlinesNames = _service.GetAllAirlinesName(), AirportsIds = _service.GetAllAirportsId() });
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = _service.GetForDelete((int)id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Search()
        {
            return View(new SearchModel { Date = DateTime.Today,CountOfPeopleWhoWillTakeTheFlight = 1,AirlinesNames = _service.GetAllAirlinesName(),AirportsIds = _service.GetAllAirportsId()});
        }
        public IActionResult SearchForSpecificFlight(SearchModel modelForSearchingFlight)
        {
            if(modelForSearchingFlight.CountOfPeopleWhoWillTakeTheFlight < 1)
            {
                return View("Search", new SearchModel {Error = 1, Date = DateTime.Today, CountOfPeopleWhoWillTakeTheFlight = 1, AirlinesNames = _service.GetAllAirlinesName(), AirportsIds = _service.GetAllAirportsId() });
            }
            var flights = _service.FindFlights(modelForSearchingFlight);
            if(flights.Count() == 0)
            {
                return View("DidntExistingFlights");
            }
            return View("Index",_service.ToFlightListModel(flights));
        }
        [HttpPost]
        public JsonResult HasThisDateRassed(DateTime TakeOff_Date)
        {

            return Json(_service.HasThisDateRassed(TakeOff_Date));

        }
    }
}
