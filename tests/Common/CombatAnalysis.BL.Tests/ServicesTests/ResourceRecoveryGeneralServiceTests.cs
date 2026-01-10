using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class ResourceRecoveryGeneralServiceTests
{
    [Fact]
    public async Task CreateBatchAsync_ShouldCreateCollectionOfEntity()
    {
        // Arrange
        var entityTakenDtoCollection = ResourceRecoveryGeneralTestDataFactory.CreateDtoCollection();
        var entityCollection = ResourceRecoveryGeneralTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<ResourceRecoveryGeneral>>();

        mockMapper.Setup(m => m.Map<IEnumerable<ResourceRecoveryGeneral>>(entityTakenDtoCollection)).Returns(entityCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new ResourceRecoveryGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityTakenDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<ResourceRecoveryGeneral>>(It.IsAny<IEnumerable<ResourceRecoveryGeneralDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<ResourceRecoveryGeneral>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
