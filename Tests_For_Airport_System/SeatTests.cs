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
    class SeatTests
    {
        private SeatsService _service;
        private AirportDbContext _context;
        private SeatsController _controller;
        private AirportsService _airport_service;
        private AirlinesService _airline_service;
        private FlightsService _flight_service;
        [SetUp]
        public void Setup()
        {

            var dbContextOptions = new DbContextOptionsBuilder<AirportDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            _context = new AirportDbContext(dbContextOptions);
            _service = new SeatsService(_context);
            _controller = new SeatsController(_service);
            _airport_service = new AirportsService(_context);
            _airline_service = new AirlinesService(_context);
            _flight_service = new FlightsService(_context);
            Airline airline = new Airline { Name = "TestName" };
            Airport airport1 = new Airport { Port_Id = "ID1", Address = "TestAdress1", Name = "TestName1", City = "TestCity1", Country = "TestCountry1" };
            Airport airport2 = new Airport { Port_Id = "ID2", Address = "TestAdress2", Name = "TestName2", City = "TestCity2", Country = "TestCountry2" };
            FlightCreateModel flight = new FlightCreateModel { AirlineName = "TestName", OriginAirportId = "ID1", DestinationAirportId = "ID2", TakeOff_Date = DateTime.Now, Landing_Date = DateTime.Now, Rows = 1, Cols = 1 };
            _airline_service.Create(airline);
            _airport_service.Create(airport1);
            _airport_service.Create(airport2);
            _flight_service.CreateFlight(flight);
        }
        //Tests For Seat Service
        [Test]
        public void GetAllMethodShouldReturnAllSeats()
        {
            //ARRANGE
            Seat seat1 = new Seat { Row = 1, Col = 'A',Seat_Class = "First",Flight_id = 1 };
            Seat seat2 = new Seat { Row = 2, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            Seat seat3 = new Seat { Row = 3, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat1);
            _service.CreateSeat(seat2);
            _service.CreateSeat(seat3);
            //ASSERT
            Assert.AreEqual(_service.GetAll(1).Count(), 3);
        }
        [Test]
        public void GetRowsMethodShouldReturnCountOfRows()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(_service.GetRows(1), 1);
        }
        [Test]
        public void GetRowsMethodShouldReturnCountOfCols()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(_service.GetCols(1), 1);
        }
        [Test]
        public void GetForDeleteShouldWorkProperly()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            DeleteAndBookSeatModel s1 = new DeleteAndBookSeatModel(1, 'A', "First", 1);
            //ACT
            _service.CreateSeat(seat);
            var s = _service.GetForDeleteOrBook(1);
            //ASSERT
            Assert.AreEqual(s1, s);
        }
        [Test]
        public void GetForDeleteShouldReturnNullIfThereIsntSeat()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            var s = _service.GetForDeleteOrBook(2);
            //ASSERT
            Assert.AreEqual(null, s);
        }
        [Test]
        public void DeleteShouldWorkProperly()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            _service.Delete(1);
            //ASSERT
            Assert.AreEqual(_service.GetAll(1).Count(), 0);
        }
        [Test]
        public void BookShouldWorkProperly()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            _service.Book(1);
            //ASSERT
            Assert.IsTrue(_service.GetAll(1).ToList()[0].isBooked);
        }

        //Tests For Seat Controller

        [Test]
        public void IndexMethodShouldReturnActionView()
        {
            //ARRANGE + ACT
            var action = _controller.Index(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void CreateMethodShouldReturnActionView()
        {
            //ARRANGE + ACT
            var action = _controller.Create(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
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
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            var action = _controller.Delete(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void ShouldDeleteRightSeat()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            var action = _controller.DeleteConfirmed(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(0, _context.Seat.Count());
        }
        [Test]
        public void ShouldReturnInBookNotFoundIfIdIsNull()
        {
            //ARRANGE + ACT
            var action = _controller.Book(null);
            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(action);
        }
        [Test]
        public void ShouldReturnInBookNotFoundIfIdDidntExist()
        {
            //ARRANGE + ACT
            var action = _controller.Book(1);
            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(action);
        }
        [Test]
        public void ShouldReturnInBookActionResultIfIdExist()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            var action = _controller.Book(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void ShouldBookRightSeat()
        {
            //ARRANGE
            Seat seat = new Seat { Row = 1, Col = 'A', Seat_Class = "First", Flight_id = 1 };
            //ACT
            _service.CreateSeat(seat);
            var action = _controller.BookConfirmed(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.IsTrue(_service.GetAll(1).ToList()[0].isBooked);
        }
    }
}