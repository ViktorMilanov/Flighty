using NUnit.Framework;
using AirportSystem.Services;
using AirportDBFirst;
using AirportSystem.Data;
using System.Linq;
using System;
using AirportSystem.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using AirportSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests_For_Airport_System
{
    public class Tests
    {
        private AirportsService _service;
        private AirportDbContext _context;
        private AirportsController _controller;
        [SetUp]
        public void Setup()
        {

            var dbContextOptions = new DbContextOptionsBuilder<AirportDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
            _context = new AirportDbContext(dbContextOptions);
            _service = new AirportsService(_context);
            _controller = new AirportsController(_service);

        }
        //Tests For Airport Service
        [Test]
        public void AddingAirportShouldBeSuccessful1()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            //ACT
            _service.Create(airport);
            //ASSERT
            Assert.AreEqual(_service.GetAll().ToList().Count, 1);
        }
        [Test]
        public void AddingNullShouldThrowException()
        {
            //ARRANGE
            Airport airport = null;
            //ACT + ASSERT 
            Assert.Throws<NullReferenceException>(() => _service.Create(airport));
        }
        [Test]
        public void AddingSameAirportTwoTimesShouldThrowException()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            //ACT
            _service.Create(airport);
            //ASSERT
            Assert.Throws<ArgumentException>(() => _service.Create(airport));
        }
        [Test]
        public void GettingNotExistingAirportShouldReturnNull()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(null, _service.GetAirport(1));
        }
        [Test]
        public void GettingExistingAirportShouldReturnRightAirport()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "Test", Name = "Test", City = "Test", Country = "Test" };
            //ACT
            _service.Create(airport);
            Airport airportToMatch = _service.GetAirport(1);
            //ASSERT
            Assert.AreEqual(airportToMatch,airport);
        }
        [Test]
        public void GettingAllAirportsShouldReturnAllAirportThatAreInDB()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "Test", Name = "Test", City = "Test", Country = "Test" };
            Airport airport1 = new Airport { Port_Id = "ID2", Address = "Test2", Name = "Test2", City = "Test2", Country = "Test2" };
            //ACT
            _service.Create(airport);
            _service.Create(airport1);
            List<AirportToDisplay> airports = _service.GetAll().ToList();
            //ASSERT
            Assert.AreEqual(airports.Count, 2);
        }
        [Test]
        public void EditingAirportShouldEditItRight()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "Test", Name = "Test", City = "Test", Country = "Test" };
            Airport airport1 = new Airport { ID = 1, Address = "Test", Name = "Test1", City = "Test", Country = "Test" };
            //ACT
            _service.Create(airport);
            _service.Edit(airport1);
            //ASSERT
            Assert.AreEqual(_service.GetAirport(1).Name, airport1.Name);
        }
        [Test]
        public void DeleteAirportShouldDeleteItRight()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "Test", Name = "Test", City = "Test", Country = "Test" };
            //ACT
            _service.Create(airport);
            _service.Delete(1);
            //ASSERT
            Assert.AreEqual(_service.GetAll().ToList().Count, 0);
        }
        [Test]
        public void WhenIsGivenExistingIndexShouldReturnRightModelWithoutId()
        {
            //ARRANGE
            Airport airport = new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            //ACT
            _service.Create(airport);
            AirportWithoutId airportModel = _service.GetForEdit(1);
            //ASSERT
            Assert.AreEqual(airportModel.Name, "TestName");
            Assert.AreEqual(airportModel.Port_Id, "ID1");
        }
        [Test]
        public void WhenIsGivenNotExistingIndexShouldReturnNull()
        {
            //ARRANGE + ACT + ASSERT
            Assert.AreEqual(null, _service.GetForEdit(1));
        }
        [Test]
        public void ShouldReturnTrueIfThereIsntAirportWithThatNameAlready()
        {
            //ARRANGE + ACT + ASSERT
            Assert.IsTrue(_service.IsThisNameAlreadyExist("TestName"));
        }
        [Test]
        public void ShouldReturnFalseIfThereIsAirportWithThatNameAlready()
        {
            //ARRANGE
            _service.Create(new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" });
            //ACT + ASSERT
            Assert.IsFalse(_service.IsThisNameAlreadyExist("TestName"));
        }
        [Test]
        public void ShouldReturnTrueIfThereIsntAirportWithThatIdAlready()
        {
            //ARRANGE + ACT + ASSERT
            Assert.IsTrue(_service.IsThisIdAlreadyExist("ID1"));
        }
        [Test]
        public void ShouldReturnFalseIfThereIsAirportWithThatIdAlready()
        {
            //ARRANGE
            _service.Create(new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" });
            //ACT + ASSERT
            Assert.IsFalse(_service.IsThisIdAlreadyExist("ID1"));
        }
        [Test]
        public void ShouldReturnTrueIfThereIsntAirportWithThatAdressAlready()
        {
            //ARRANGE + ACT + ASSERT
            Assert.IsTrue(_service.IsThisAdressAlreadyExist("TestAdress"));
        }
        [Test]
        public void ShouldReturnFalseIfThereIsAirportWithThatAdressAlready()
        {
            //ARRANGE
            _service.Create(new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" });
            //ACT + ASSERT
            Assert.IsFalse(_service.IsThisAdressAlreadyExist("TestAdress"));
        }

        //Tests For Airport Controller


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
            Airport airport = new Airport { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            _service.Create(airport);
            //ACT
            var action = _controller.Edit(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void CreateMethodShouldCreateRightAirport()
        {
            //ARRANGE
            AirportWithoutId airport = new AirportWithoutId { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            //ACT
            var action = _controller.Create(airport);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(_service.GetAll().ToList().Count, 1);
        }
        [Test]
        public void EditMethodShouldEditRightAirport()
        {
            //ARRANGE
            AirportWithoutId airport = new AirportWithoutId { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            Airport airportForEdit = new Airport { ID = 1, Address = "TestAdress", Name = "TestName1", City = "TestCity", Country = "TestCountry" };
            //ACT
            _controller.Create(airport);
            var action = _controller.Edit(airportForEdit);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
            Assert.AreEqual(_service.GetAirport(1).Name, "TestName1");
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
            AirportWithoutId airport = new AirportWithoutId { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            //ACT
            _controller.Create(airport);
            var action = _controller.Delete(1);
            //ASSERT
            Assert.IsInstanceOf<IActionResult>(action);
        }
        [Test]
        public void ShouldDeleteRightAirport()
        {
            //ARRANGE
            AirportWithoutId airport = new AirportWithoutId { Port_Id = "ID1", Address = "TestAdress", Name = "TestName", City = "TestCity", Country = "TestCountry" };
            //ACT
            _controller.Create(airport);
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
        [Test]
        public void IsThisIdAlreadyExistMethodShouldReturnJsonResult()
        {
            //ARRANGE + ACT
            var action = _controller.IsThisIdAlreadyExist("Id");
            //ASSERT
            Assert.IsInstanceOf<JsonResult>(action);
        }
        [Test]
        public void IsThisAdressAlreadyExistMethodShouldReturnJsonResult()
        {
            //ARRANGE + ACT
            var action = _controller.IsThisAdressAlreadyExist("Adress");
            //ASSERT
            Assert.IsInstanceOf<JsonResult>(action);
        }
    }
}