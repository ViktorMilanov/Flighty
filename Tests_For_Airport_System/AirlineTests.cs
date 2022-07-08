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
    class AirlineTests
    {
        private AirlinesService _service;
        private AirportDbContext _context;
        private AirlinesController _controller;
        [SetUp]
        public void Setup()
        {

            var dbContextOptions = new DbContextOptionsBuilder<AirportDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            _context = new AirportDbContext(dbContextOptions);
            _service = new AirlinesService(_context);
            _controller = new AirlinesController(_service);

        }
        //Tests For Airport Service
        [Test]
        public void AddingAirlineShouldBeSuccessful1()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            //ACT
            _service.Create(airline);
            //ASSERT
            Assert.AreEqual(_service.GetAll().ToList().Count, 1);
        }
        [Test]
        public void AddingNullShouldThrowException()
        {
            //ARRANGE
            Airline airline = null;
            //ACT + ASSERT 
            Assert.Throws<NullReferenceException>(() => _service.Create(airline));
        }
        [Test]
        public void AddingSameAirlineTwoTimesShouldThrowException()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            //ACT
            _service.Create(airline);
            //ASSERT
            Assert.Throws<ArgumentException>(() => _service.Create(airline));
        }
        [Test]
        public void GettingNotExistingAirlineShouldReturnNull()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(null, _service.GetAirline(1));
        }
        [Test]
        public void GettingExistingAirlineShouldReturnRightAirline()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            //ACT
            _service.Create(airline);
            Airline AirlineToMatch = _service.GetAirline(1);
            //ASSERT
            Assert.AreEqual(AirlineToMatch, airline);
        }
        [Test]
        public void GettingAllAirlinesShouldReturnAllAirlineThatAreInDB()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            Airline airline1 = new Airline { Name = "TestName1" };
            //ACT
            _service.Create(airline);
            _service.Create(airline1);
            List<Airline> Airlines = _service.GetAll().ToList();
            //ASSERT
            Assert.AreEqual(Airlines.Count, 2);
        }
        [Test]
        public void EditingAirlineShouldEditItRight()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            Airline airline1 = new Airline {Id = 1, Name = "TestName1" };
            //ACT
            _service.Create(airline);
            _service.Edit(airline1);
            //ASSERT
            Assert.AreEqual(_service.GetAirline(1).Name, airline1.Name);
        }
        [Test]
        public void DeleteAirlineShouldDeleteItRight()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            //ACT
            _service.Create(airline);
            _service.Delete(1);
            //ASSERT
            Assert.AreEqual(_service.GetAll().ToList().Count, 0);
        }
        [Test]
        public void WhenIsGivenExistingIndexShouldReturnRightModelWithoutId()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            //ACT
            _service.Create(airline);
            AirlineToDisplay AirlineModel = _service.GetForEdit(1);
            //ASSERT
            Assert.AreEqual(AirlineModel.Name, "TestName");
        }
        [Test]
        public void WhenIsGivenNotExistingIndexShouldReturnNull()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(null, _service.GetForEdit(1));
        }
        [Test]
        public void ShouldReturnTrueIfThereIsntAirlineWithThatNameAlready()
        {
            //ARRANGE + ACT + ASSERT
            Assert.IsTrue(_service.IsThisNameAlreadyExist("TestName"));
        }
        [Test]
        public void ShouldReturnFalseIfThereIsAirlineWithThatNameAlready()
        {
            //ARRANGE
            _controller.Create(new AirlineToDisplay { Name = "TestName" });
            //ACT + ASSERT
            Assert.IsFalse(_service.IsThisNameAlreadyExist("TestName"));
        }

        //Tests For Airline Controller


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
        public void EditMethodShouldReturnNotFoundIfIndexDidntExist()
        {
            //ARRANGE + ACT
            var action = _controller.Edit(1);
            //ASSERT
            Assert.IsInstanceOf<NotFoundResult>(action);
        }
        [Test]
        public void EditMethodShouldReturnViewIfIndexExist()
        {
            //ARRANGE
            Airline airline = new Airline { Name = "TestName" };
            _service.Create(airline);
            //ACT
            var action = _controller.Edit(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void CreateMethodShouldCreateRightAirline()
        {
            //ARRANGE
            AirlineToDisplay air = new AirlineToDisplay { Name = "TestName" };
            //ACT
            var action = _controller.Create(air);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(_service.GetAll().ToList().Count, 1);
        }
        [Test]
        public void EditMethodShouldEditRightAirline()
        {
            //ARRANGE
            AirlineToDisplay airline = new AirlineToDisplay { Name = "TestName" };
            Airline airlineForEdit = new Airline {Id = 1, Name = "TestName1"};
            //ACT
            _controller.Create(airline);
            var action = _controller.Edit(airlineForEdit);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(_service.GetAirline(1).Name, "TestName1");
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
            AirlineToDisplay airline = new AirlineToDisplay { Name = "TestName" };
            //ACT
            _controller.Create(airline);
            var action = _controller.Delete(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void ShouldDeleteRightAirline()
        {
            //ARRANGE
            AirlineToDisplay airline = new AirlineToDisplay { Name = "TestName" };
            //ACT
            _controller.Create(airline);
            var action = _controller.DeleteConfirmed(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(0, _service.GetAll().ToList().Count);
        }
        [Test]
        public void IsThisNameAlreadyExistMethodShouldReturnJsonResult()
        {
            //ARRANGE + ACT
            var action = _controller.IsThisNameAlreadyExist("Name");
            //ASSERT
            Assert.IsInstanceOf<JsonResult>(action);
        }
        
    }
}
