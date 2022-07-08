using Microsoft.AspNetCore.Mvc;
using AirportSystem.Data;
using AirportSystem.Services;
using AirportSystem.Models;

namespace AirportSystem.Controllers
{
    public class AirlinesController : Controller
    {
        private readonly AirlinesService _service;

        public AirlinesController(AirlinesService service)
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
        public IActionResult Create(AirlineToDisplay airline)
        {
            if (ModelState.IsValid)
            {
                _service.Create(new Airline {Name = airline.Name});
                return RedirectToAction(nameof(Index));
            }
            return View(airline);
        }
        public IActionResult Edit(int id)
        {
            var airline = _service.GetAirline(id);
            if (airline == null) return NotFound();
            return View(airline);
        }
        [HttpPost]
        public IActionResult Edit(Airline airline)
        {
            _service.Edit(airline);
            return RedirectToAction("Index", "Airlines");
        }

        // GET: Airports/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airline = _service.GetForEdit((int)id);
            if (airline == null)
            {
                return NotFound();
            }

            return View(airline);
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
    }
}
