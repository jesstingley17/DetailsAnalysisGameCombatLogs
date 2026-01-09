using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageDoneServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDtoCollection = DamageDoneTestDataFactory.CreateDtoCollection();
        var entityCollection = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDone>>(entityDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(entityCollection)).Returns(entityDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new DamageDoneService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<DamageDone>>(It.IsAny<IEnumerable<DamageDoneDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<DamageDone>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<DamageDoneDto>>(It.IsAny<IEnumerable<DamageDone>>()), Times.Once);
    }
}
