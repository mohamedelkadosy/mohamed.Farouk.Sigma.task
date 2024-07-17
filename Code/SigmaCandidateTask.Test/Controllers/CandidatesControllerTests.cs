using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SigmaCandidateTask.API.Controllers;
using SigmaCandidateTask.Core.IServices;
using SigmaCandidateTask.Core.ViewModels.Candidate;
using System;
using System.Threading.Tasks;

namespace SigmaCandidateTask.Tests.Controllers
{
    [TestFixture]
    public class CandidatesControllerTests
    {
        private Mock<ICandidateServices> _candidateServiceMock;
        private CandidatesController _candidatesController;

        [SetUp]
        public void Setup()
        {
            _candidateServiceMock = new Mock<ICandidateServices>();
            _candidatesController = new CandidatesController(_candidateServiceMock.Object);
        }

        [Test]
        public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            _candidatesController.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await _candidatesController.AddOrUpdateCandidate(new CandidateViewModel());

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task AddOrUpdateCandidate_ReturnsOk_WhenModelIsValid()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "New candidate"
            };

            _candidateServiceMock.Setup(service => service.AddOrUpdateAsync(candidateViewModel))
                                 .Returns(Task.CompletedTask);

            // Act
            var result = await _candidatesController.AddOrUpdateCandidate(candidateViewModel);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual("Candidate added or updated successfully.", okResult.Value);
        }

        [Test]
        public async Task AddOrUpdateCandidate_CallsServiceMethod_WhenModelIsValid()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "New candidate"
            };

            _candidateServiceMock.Setup(service => service.AddOrUpdateAsync(candidateViewModel))
                                 .Returns(Task.CompletedTask);

            // Act
            await _candidatesController.AddOrUpdateCandidate(candidateViewModel);

            // Assert
            _candidateServiceMock.Verify(service => service.AddOrUpdateAsync(candidateViewModel), Times.Once);
        }

    
    }
}
