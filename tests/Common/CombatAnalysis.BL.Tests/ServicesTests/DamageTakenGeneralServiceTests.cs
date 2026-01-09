using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageTakenGeneralServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityTakenDtoCollection = DamageTakenGeneralTestDataFactory.CreateDtoCollection();
        var entityCollection = DamageTakenGeneralTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<DamageTakenGeneral>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageTakenGeneral>>(entityTakenDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<DamageTakenGeneralDto>>(entityCollection)).Returns(entityTakenDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new DamageTakenGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityTakenDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<DamageTakenGeneral>>(It.IsAny<IEnumerable<DamageTakenGeneralDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<DamageTakenGeneral>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<DamageTakenGeneralDto>>(It.IsAny<IEnumerable<DamageTakenGeneral>>()), Times.Once);
    }
}
