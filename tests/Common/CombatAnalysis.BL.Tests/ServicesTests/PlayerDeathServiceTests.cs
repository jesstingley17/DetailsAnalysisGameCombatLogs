using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class PlayerDeathServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityDtoCollection = PlayerDeathTestDataFactory.CreateDtoCollection();
        var entityCollection = PlayerDeathTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<CombatPlayerDeath>>();

        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayerDeath>>(entityDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<CombatPlayerDeathDto>>(entityCollection)).Returns(entityDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new PlayerDeathService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<CombatPlayerDeath>>(It.IsAny<IEnumerable<CombatPlayerDeathDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<CombatPlayerDeath>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<CombatPlayerDeathDto>>(It.IsAny<IEnumerable<CombatPlayerDeath>>()), Times.Once);
    }
}
