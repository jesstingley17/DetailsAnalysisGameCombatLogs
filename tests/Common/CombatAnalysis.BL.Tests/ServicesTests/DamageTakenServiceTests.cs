using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageTakenServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityTakenDtoCollection = DamageTakenTestDataFactory.CreateDtoCollection();
        var entityCollection = DamageTakenTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<DamageTaken>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageTaken>>(entityTakenDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<DamageTakenDto>>(entityCollection)).Returns(entityTakenDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new DamageTakenService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityTakenDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<DamageTaken>>(It.IsAny<IEnumerable<DamageTakenDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<DamageTaken>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<DamageTakenDto>>(It.IsAny<IEnumerable<DamageTaken>>()), Times.Once);
    }
}
