using Microsoft.AspNetCore.Mvc;
using AirportSystem.Data;
using AirportSystem.Models;
using AirportSystem.Services;

namespace AirportSystem.Controllers
{
    public class AirportsController : Controller
    {
        private readonly AirportsService _service;

        public AirportsController(AirportsService service)
        {
            _service = service;
        }

        // GET: Airports
        public IActionResult Index()
        {
            return View(_service.GetAll());
        }

        // GET: Airports/Create  
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AirportWithoutId airport)
        {
            if (ModelState.IsValid)
            {
                _service.Create(new Airport { Port_Id = airport.Port_Id, Name = airport.Name, Address = airport.Address, City = airport.City, Country = airport.Country });
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }
        public IActionResult Edit(int id)
        {
            var airport = _service.GetAirport(id);
            if (airport == null) return NotFound();
            return View(airport);
        }
        [HttpPost]
        public IActionResult Edit(Airport airport)
        {
            _service.Edit(airport);
            return RedirectToAction("Index", "Airports");
        }

        // GET: Airports/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = _service.GetForEdit((int)id);
            if (airport == null)
            {
                return NotFound();
            }

            return View(airport);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            _service.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public JsonResult IsThisNameAlreadyExist(string Name)
        {

            return Json(_service.IsThisNameAlreadyExist(Name));

        }
        [HttpPost]
        public JsonResult IsThisIdAlreadyExist(string Port_Id)
        {

            return Json(_service.IsThisIdAlreadyExist(Port_Id));

        }
        [HttpPost]
        public JsonResult IsThisAdressAlreadyExist(string Adress)
        {

            return Json(_service.IsThisAdressAlreadyExist(Adress));

        }
    }
}