using AirportDBFirst;
using AirportSystem.Controllers;
using AirportSystem.Data;
using AirportSystem.Models;
using AirportSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests_For_Airport_System
{
    class FlightTests
    {
        private FlightsService _service;
        private AirportDbContext _context;
        private FlightsController _controller;
        private AirportsService _airport_service;
        private AirlinesService _airline_service;
        private SeatsService _seat_service;
        [SetUp]
        public void Setup()
        {

            var dbContextOptions = new DbContextOptionsBuilder<AirportDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            _context = new AirportDbContext(dbContextOptions);
            _service = new FlightsService(_context);
            _controller = new FlightsController(_service);
            _airport_service = new AirportsService(_context);
            _airline_service = new AirlinesService(_context);
            _seat_service = new SeatsService(_context);
            Airline airline = new Airline { Name = "TestName" };
            Airport airport1 = new Airport { Port_Id = "ID1", Address = "TestAdress1", Name = "TestName1", City = "TestCity1", Country = "TestCountry1" };
            Airport airport2 = new Airport { Port_Id = "ID2", Address = "TestAdress2", Name = "TestName2", City = "TestCity2", Country = "TestCountry2" };
            _airline_service.Create(airline);
            _airport_service.Create(airport1);
            _airport_service.Create(airport2);
        }
        //Tests For Airport Service
        [Test]
        public void AddingFlightShouldBeSuccessful()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            _service.CreateFlight(flight);
            //ASSERT
            Assert.AreEqual(_context.Flight.Count(), 1);
        }
        [Test]
        public void GetAllMethodShouldReturnAllFlights()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            _service.CreateFlight(flight);
            _service.CreateFlight(flight);
            _service.CreateFlight(flight);
            //ASSERT
            Assert.AreEqual(_service.GetAll(_context.Flight.ToList()).Count(), 3);
        }
        [Test]
        public void EditedFlightsShouldRemoveUnwantedSeats()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            _service.CreateFlight(flight);
            _seat_service.CreateSeat(new Seat { Flight_id = 1, Col = 'A', Row = 1, Seat_Class = "First" });
            _seat_service.CreateSeat(new Seat { Flight_id = 1, Col = 'A', Row = 2, Seat_Class = "First" });
            _service.EditedFlights();
            //ASSERT
            Assert.AreEqual(_context.Seat.Where(s => s.Flight_id == 1).Count(), 1);
        }
        [Test]
        public void GetForDeleteShouldGetRightFlightAndTransformItToFlightDeleteModel()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            FlightDeleteModel flight1 = new FlightDeleteModel("TestName", "ID1", "ID2", DateTime.Now, DateTime.Now, 1, 1);
            //ACT
            _service.CreateFlight(flight);
            var flight2 = _service.GetForDelete(1);
            //ASSERT
            Assert.AreEqual(flight1.AirlineName, flight2.AirlineName);
            Assert.AreEqual(flight1.OriginAirportId, flight2.OriginAirportId);
            Assert.AreEqual(flight1.DestinationAirportId, flight2.DestinationAirportId);
            Assert.AreEqual(flight1.Rows, flight2.Rows);
            Assert.AreEqual(flight1.Cols, flight2.Cols);
        }
        [Test]
        public void GetForDeleteShouldGetReturnNullWhenThereIsntSuchAFlight()
        {
            //ARRANGE + ACT
            var flight2 = _service.GetForDelete(1);
            //ASSERT
            Assert.AreEqual(flight2, null);
        }
        [Test]
        public void SearchFlightShouldReturnListWithRightFlights()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.MinValue, Rows = 1, Cols = 1 };
            FlightCreateModel flight1 = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.MinValue, Rows = 1, Cols = 1 };
            SearchModel flightModel = new SearchModel {Date = DateTime.MinValue, DestinationAirportId="ID2", OriginAirportId = "ID1", CountOfPeopleWhoWillTakeTheFlight=1};
            //ACT
            _service.CreateFlight(flight);
            _service.CreateFlight(flight1);
            _seat_service.CreateSeat(new Seat { Flight_id = 1, Col = 'A', Row = 1, Seat_Class = "First" });
            var flights = _service.FindFlights(flightModel);
            //ASSERT
            Assert.AreEqual(flights.Count(), 1);
        }
        [Test]
        public void ToListModelShouldTransformFlightsToListModel()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.MinValue, Rows = 1, Cols = 1 };
            FlightListModel flight1 = new FlightListModel("TestName", "ID1", "ID2", DateTime.MinValue, DateTime.MinValue, 0, 0,1);
            //ACT
            _service.CreateFlight(flight);
            var flights = _service.ToFlightListModel(_context.Flight.ToList()).ToList()[0];
            //ASSERT
            Assert.AreEqual(flight1.AirlineName, flights.AirlineName);
            Assert.AreEqual(flight1.OriginAirportId, flights.OriginAirportId);
            Assert.AreEqual(flight1.DestinationAirportId, flights.DestinationAirportId);
            Assert.AreEqual(flight1.AllSeats, flights.AllSeats);
            Assert.AreEqual(flight1.FreeSeats, flights.FreeSeats);
        }
        [Test]
        public void DeleteFlightShouldBeSuccessful()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            _service.CreateFlight(flight);
            _service.Delete(1);
            //ASSERT
            Assert.AreEqual(_context.Flight.Count(), 0);
        }
        [Test]
        public void GetAllAirlinesShouldReturnAllAirlinesNames()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(_service.GetAllAirlinesName().Count(), 1);
        }
        [Test]
        public void GetAllAirportsShouldReturnAllAirportsIds()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(_service.GetAllAirportsId().Count(), 2);
        }
        [Test]
        public void MethodForDataValidationShouldReturnFalseWhenDateIsPassed()
        {
            //ARRANGE + ACT + ASSERT
            Assert.IsFalse(_service.HasThisDateRassed(DateTime.Today));
        }
        [Test]
        public void MethodForDataValidationShouldReturnTrueWhenDateIstPassed()
        {
            //ARRANGE + ACT + ASSERT
            Assert.IsTrue(_service.HasThisDateRassed(DateTime.MaxValue));
        }


        //Tests For Flight Controller


        [Test]
        public void IndexMethodShouldReturnActionView()
        {
            //ARRANGE + ACT
            var action = _controller.Index();
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void CreateMethodShouldReturnActionView()
        {
            //ARRANGE + ACT
            var action = _controller.Create();
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        
        [Test]
        public void CreateMethodShouldCreateRightAirline()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            var action = _controller.Create(flight);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(_context.Flight.ToList().Count, 1);
        }
        [Test]
        public void CreateMethodReturnErrorWhenLandingDateIsEarlierThanTakeoff()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.MinValue, Rows = 1, Cols = 1 };
            //ACT
            var action = _controller.Create(flight);
            //ASSERT
            Assert.AreEqual(_context.Flight.ToList().Count, 0);
        }
        [Test]
        public void CreateMethodReturnErrorWhenTwoAirportsAreSame()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID1", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            var action = _controller.Create(flight);
            //ASSERT
            Assert.AreEqual(_context.Flight.ToList().Count, 0);
        }
        [Test]
        public void ShouldReturnNotFoundIfIdIsNull()
        {
            //ARRANGE + ACT
            var action = _controller.Delete(null);
            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(action);
        }
        [Test]
        public void ShouldReturnNotFoundIfIdDidntExist()
        {
            //ARRANGE + ACT
            var action = _controller.Delete(1);
            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(action);
        }
        [Test]
        public void ShouldReturnActionResultIfIdExist()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            _controller.Create(flight);
            var action = _controller.Delete(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void ShouldDeleteRightFlight()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            //ACT
            _controller.Create(flight);
            var action = _controller.DeleteConfirmed(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(0, _context.Flight.Count());
        }
        [Test]
        public void SearchMethodShouldReturnActionResult()
        {
            //ARRANGE + ACT
            var action = _controller.Search();
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
    [Test]
    public void SearchForSpecificFlightShouldReturnViewWhenCountOfPeopleAreLessThanOne()
    {
        //ARRANGE
        FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
        SearchModel flightModel = new SearchModel { Date = DateTime.MinValue, DestinationAirportId = "ID2", OriginAirportId = "ID1", CountOfPeopleWhoWillTakeTheFlight = 0 };
        //ACT
        _controller.Create(flight);
            var action = _controller.SearchForSpecificFlight(flightModel);
        //ASSERT
        Assert.IsInstanceOf<IActionResult>(action);
    }
        [Test]
        public void SearchForSpecificFlightShouldReturnViewWhenThereIsntSuchAFlight()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            SearchModel flightModel = new SearchModel { Date = DateTime.MinValue, DestinationAirportId = "ID1", OriginAirportId = "ID2", CountOfPeopleWhoWillTakeTheFlight = 1 };
            //ACT
            _controller.Create(flight);
            var action = _controller.SearchForSpecificFlight(flightModel);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void SearchForSpecificFlightShouldReturnRightViewWhenThereIsSuchAFlight()
        {
            //ARRANGE
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.MinValue, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            SearchModel flightModel = new SearchModel { Date = DateTime.MinValue, DestinationAirportId = "ID2", OriginAirportId = "ID1", CountOfPeopleWhoWillTakeTheFlight = 1 };
            //ACT
            _controller.Create(flight);
            _seat_service.CreateSeat(new Seat { Flight_id = 1, Col = 'A', Row = 1, Seat_Class = "First" });
            var action = _controller.SearchForSpecificFlight(flightModel);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void IfThisDateIsPassedMethodShouldReturnJsonResult()
        {
            //ARRANGE + ACT
            var action = _controller.HasThisDateRassed(DateTime.MinValue);
            //ASSERT
            Assert.IsInstanceOf<JsonResult>(action);
        }
    }
}
