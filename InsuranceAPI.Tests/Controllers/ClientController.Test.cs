using AutoMapper;
using InsuranceAPI.Controllers;
using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace InsuranceAPI.Tests.Controllers
{
    [TestFixture]
    public class ClientControllerTest
    {
        private Mock<IClientsService> _clientsService;
        private Mock<IUserService> _userService;
        private ClientController _controller;

        [OneTimeSetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<ClientController>>().Object;
            var mockMapper = new Mock<IMapper>(MockBehavior.Loose).Object;
            var mockSettings = new Mock<IOptions<InsuranceAPI.Infrastructure.Models.MongoDatabaseSettings>>().Object;
            _clientsService = new Mock<IClientsService>(MockBehavior.Loose);
            _userService = new Mock<IUserService>(MockBehavior.Loose);
            _controller = new ClientController(mockLogger, mockMapper, mockSettings, _clientsService.Object, _userService.Object);
        }

        [Test]
        public async Task GetAll_WithClients_ReturnJsonClients()
        {
            var result = await _controller.GetAll();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(JsonResult)));
        }

        [Test]
        public async Task GetBy_WithValidId_ReturnJsonClient()
        {
            var id = ObjectId.GenerateNewId(DateTime.UtcNow).ToString();
            Client client = new();
            var userId = ObjectId.GenerateNewId(DateTime.Now);
            client.UserId = new MongoDBRef("Users", userId);
            User user = new()
            {
                Id = userId
            };

            _clientsService.Setup(r => r.GetById(It.IsAny<string>())).ReturnsAsync(client);
            _userService.Setup(r => r.GetById(It.IsAny<ObjectId>())).ReturnsAsync(user);

            var result = await _controller.GetById(id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.GetType(), Is.EqualTo(typeof(JsonResult)));
        }
    }
}
