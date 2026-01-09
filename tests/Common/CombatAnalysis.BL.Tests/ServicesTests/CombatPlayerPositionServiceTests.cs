using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatPlayerPositionServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDtoCollection = CombatPlayerPositionTestDataFactory.CreateDtoCollection();
        var entityCollection = CombatPlayerPositionTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<CombatPlayerPosition>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayerPosition>>(entityDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayerPositionDto>>(entityCollection)).Returns(entityDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new CombatPlayerPositionService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<CombatPlayerPosition>>(It.IsAny<IEnumerable<CombatPlayerPositionDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<CombatPlayerPosition>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<CombatPlayerPositionDto>>(It.IsAny<IEnumerable<CombatPlayerPosition>>()), Times.Once);
    }
}
