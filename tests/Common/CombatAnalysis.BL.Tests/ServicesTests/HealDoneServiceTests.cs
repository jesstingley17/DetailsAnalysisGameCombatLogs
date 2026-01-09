using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class HealDoneServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDtoCollection = HealDoneTestDataFactory.CreateDtoCollection();
        var entityCollection = HealDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<HealDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<HealDone>>(entityDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<HealDoneDto>>(entityCollection)).Returns(entityDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new HealDoneService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<HealDone>>(It.IsAny<IEnumerable<HealDoneDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<HealDone>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<HealDoneDto>>(It.IsAny<HealDone>()), Times.Once);
    }
}
