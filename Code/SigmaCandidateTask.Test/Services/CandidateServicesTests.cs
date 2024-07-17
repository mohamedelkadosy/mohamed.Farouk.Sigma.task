using AutoMapper;
using Moq;
using NUnit.Framework;
using SigmaCandidateTask.Application.Services;
using SigmaCandidateTask.Core.IRepositories;
using SigmaCandidateTask.Core.ViewModels.Candidate;
using SigmaCandidateTask.Entity;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SigmaCandidateTask.Tests.Services
{
    [TestFixture]
    public class CandidateServicesTests
    {
        private Mock<ICandidateRepositoryAsync> _candidateRepositoryMock;
        private Mock<IUnitOfWorkAsync> _unitOfWorkMock;
        private IMapper _mapper;
        private CandidateServices _candidateServices;

        [SetUp]
        public void Setup()
        {
            _candidateRepositoryMock = new Mock<ICandidateRepositoryAsync>();
            _unitOfWorkMock = new Mock<IUnitOfWorkAsync>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CandidateViewModel, Candidate>().ReverseMap();
            });

            _mapper = config.CreateMapper();
            _candidateServices = new CandidateServices(_candidateRepositoryMock.Object, _unitOfWorkMock.Object, _mapper);
        }

        [Test]
        public async Task AddOrUpdateAsync_AddsNewCandidate_WhenCandidateDoesNotExist()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "New candidate"
            };

            _candidateRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
                                    .ReturnsAsync((Candidate)null);

            _candidateRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Candidate>()))
                                    .ReturnsAsync(new Candidate());

            // Act
            await _candidateServices.AddOrUpdateAsync(candidateViewModel);

            // Assert
            _candidateRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Candidate>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task AddOrUpdateAsync_UpdatesExistingCandidate_WhenCandidateExists()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "Updated candidate"
            };

            var existingCandidate = new Candidate { Id = 1, Email = "john.doe@example.com" };

            _candidateRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
                                    .ReturnsAsync(existingCandidate);

            _candidateRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Candidate>()))
                                    .ReturnsAsync(existingCandidate);

            // Act
            await _candidateServices.AddOrUpdateAsync(candidateViewModel);

            // Assert
            _candidateRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Candidate>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Test]
        public void ValidateModelAsync_ThrowsException_WhenEmailExists()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                Id = 1,
                Email = "existing@example.com"
            };

            var existingCandidate = new Candidate { Id = 2, Email = "existing@example.com" };

            _candidateRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
                                    .ReturnsAsync(existingCandidate);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _candidateServices.ValidateModelAsync(candidateViewModel));
            Assert.That(ex.Message, Is.EqualTo("This email is already in use."));
        }

        [Test]
        public void AddOrUpdateAsync_ThrowsException_WhenCandidateNotFoundForUpdate()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "Updated candidate"
            };

            _candidateRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
                                    .ReturnsAsync((Candidate)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _candidateServices.AddOrUpdateAsync(candidateViewModel));
            Assert.That(ex.Message, Is.EqualTo("Candidate not found."));
        }

        [Test]
        public async Task AddOrUpdateAsync_CommitsUnitOfWork_AfterAddingNewCandidate()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "New candidate"
            };

            _candidateRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
                                    .ReturnsAsync((Candidate)null);

            _candidateRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Candidate>()))
                                    .ReturnsAsync(new Candidate());

            // Act
            await _candidateServices.AddOrUpdateAsync(candidateViewModel);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }

        [Test]
        public async Task AddOrUpdateAsync_CommitsUnitOfWork_AfterUpdatingExistingCandidate()
        {
            // Arrange
            var candidateViewModel = new CandidateViewModel
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Comment = "Updated candidate"
            };

            var existingCandidate = new Candidate { Id = 1, Email = "john.doe@example.com" };

            _candidateRepositoryMock.Setup(repo => repo.FirstOrDefaultAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
                                    .ReturnsAsync(existingCandidate);

            _candidateRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Candidate>()))
                                    .ReturnsAsync(existingCandidate);

            // Act
            await _candidateServices.AddOrUpdateAsync(candidateViewModel);

            // Assert
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(), Times.Once);
        }
    }
}
