using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuranceAPI.Tests.Infrastructure.Services
{
    [TestFixture]
    public class InsurancePoliciesServiceTest
    {
        private Mock<IInsurancePoliciesRepository> _mockRepository;
        private InsurancePoliciesService _service;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IInsurancePoliciesRepository>(MockBehavior.Strict);
            _service = new InsurancePoliciesService(_mockRepository.Object);
        }

        [Test]
        public async Task Create_ReceiveInsurancePolicy_ShoudlRunAddMethod()
        {
            var insurancePolicy = new InsurancePolicy();

            _service.Create(insurancePolicy);

            _mockRepository.Verify(x => x.Add(insurancePolicy), Times.Once());
        }

        [Test]
        public async Task GetAll_WithInsurancePolicies_ReturnAllInsurancePolicies()
        {
            _mockRepository.Setup(r => r.GetAll())
                .ReturnsAsync(new List<InsurancePolicy>()
                {
                    new InsurancePolicy(),
                }.AsEnumerable());

            var actual = await _service.GetAll();

            Assert.That(actual.Count(), Is.EqualTo(1));
            _mockRepository.Verify(x => x.GetAll(), Times.Once());
        }

        [Test]
        public async Task GetById_WithValidId_ShouldReturnInsurancePolicy()
        {
            var objectId = ObjectId.GenerateNewId(DateTime.Now);
            var id = objectId.ToString();

            _mockRepository.Setup(r => r.GetById(It.IsAny<ObjectId>()))
                .ReturnsAsync(new InsurancePolicy());

            var insurancePolicy = await _service.GetById(id);

            Assert.That(insurancePolicy, Is.Not.Null);
            _mockRepository.Verify(x => x.GetById(objectId), Times.Once());
        }
    }
}
