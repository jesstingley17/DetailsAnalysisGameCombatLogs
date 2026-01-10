using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using CombatAnalysis.BL.Tests.Factory;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class CombatAuraServiceTests
{
    [Fact]
    public async Task CreateBatchAsync_ShouldCreateCollectionOfEntity()
    {
        // Arrange
        var entityDtoCollection = CombatAuraTestDataFactory.CreateDtoCollection();
        var entityCollection = CombatAuraTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<CombatAura>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatAura>>(entityDtoCollection)).Returns(entityCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new CombatAuraService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<CombatAura>>(It.IsAny<IEnumerable<CombatAuraDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<CombatAura>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
