using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class HealDoneGeneralServiceTests
{
    [Fact]
    public async Task CreateBatchAsync_ShouldCreateCollectionOfEntity()
    {
        // Arrange
        var entityTakenDtoCollection = HealDoneGeneralTestDataFactory.CreateDtoCollection();
        var entityCollection = HealDoneGeneralTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<HealDoneGeneral>>();

        mockMapper.Setup(m => m.Map<IEnumerable<HealDoneGeneral>>(entityTakenDtoCollection)).Returns(entityCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new HealDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
       await service.CreateBatchAsync(entityTakenDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<HealDoneGeneral>>(It.IsAny<IEnumerable<HealDoneGeneralDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<HealDoneGeneral>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
