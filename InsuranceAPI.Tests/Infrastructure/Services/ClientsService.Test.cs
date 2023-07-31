using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;

namespace InsuranceAPI.Tests.Infrastructure.Services
{
    [TestFixture]
    public class ClientsServiceTest
    {
        private IClientsService _service;
        private Mock<IClientsRepository> _mockRepository;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IClientsRepository>(MockBehavior.Strict);
            _service = new ClientsService(_mockRepository.Object);
        }

        [Test]
        public async Task Create_WithClientEntity_ShouldCallAddMethod()
        {
            _mockRepository.Setup(r => r.Add(It.IsAny<Client>())).Returns(Task.CompletedTask);

            var client = new Client();
            await _service.Create(client);

            _mockRepository.Verify(x => x.Add(client), Times.Once());
        }

        [Test]
        public async Task GetAll_WithClients_ShouldReturnListOfClients()
        {
            _mockRepository.Setup(r => r.GetAll())
                .ReturnsAsync(new List<Client>()
                {
                    new Client
                    {
                        ContactInformation = new ContactInformation(),
                        DateOfBirth = DateTime.UtcNow,
                        FamilyName = "Foo",
                        GivenName = "Bar",
                        Id = ObjectId.GenerateNewId(),
                        Identification = "123456",
                        IdentificationType = "Type",
                        UserId = new MongoDBRef("Users", ObjectId.GenerateNewId()),
                    },
                }.AsEnumerable());

            var actual = await _service.GetAll();

            Assert.That(actual.Count(), Is.EqualTo(1));
            _mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Test]
        public async Task GetById_WithValidId_ShouldReturnClient()
        {            
            var objectId = ObjectId.GenerateNewId(DateTime.Now);
            var id = objectId.ToString();
            var identification = "123456";
            _mockRepository.Setup(r => r.GetById(It.IsAny<ObjectId>()))
                .ReturnsAsync(new Client
                {
                    ContactInformation = new ContactInformation(),
                    DateOfBirth = DateTime.UtcNow,
                    FamilyName = "Foo",
                    GivenName = "Bar",
                    Id = objectId,
                    Identification = identification,
                    IdentificationType = "Type",
                    UserId = new MongoDBRef("Users", ObjectId.GenerateNewId(DateTime.Now)),
                });

            var client = await _service.GetById(id);

            Assert.That(client, Is.Not.Null);
            Assert.That(client.Identification, Is.EqualTo(identification));
            _mockRepository.Verify(x => x.GetById(objectId), Times.Once());
        }
    }
}
