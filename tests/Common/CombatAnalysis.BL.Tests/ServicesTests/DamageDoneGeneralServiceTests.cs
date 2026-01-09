using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities.CombatPlayerData;
using CombatAnalysis.DAL.Interfaces.Generic;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests;

public class DamageDoneGeneralServiceTests
{
    [Fact]
    public async Task CreateAsync_CreatedEntity_ShouldCreateEntityAndReturnCreatedEntity()
    {
        // Arrange
        var entityGeneralDtoCollection = DamageDoneGeneralTestDataFactory.CreateDtoCollection();
        var entityCollection = DamageDoneGeneralTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<ICreateBatchRepository<DamageDoneGeneral>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneGeneral>>(entityGeneralDtoCollection)).Returns(entityCollection);
        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneGeneralDto>>(entityCollection)).Returns(entityGeneralDtoCollection);

        mockRepository.Setup(m => m.CreateBatchAsync(entityCollection, CancellationToken.None)).Returns(Task.CompletedTask);

        var service = new DamageDoneGeneralService(mockRepository.Object, mockMapper.Object);

        // Act
        await service.CreateBatchAsync(entityGeneralDtoCollection, CancellationToken.None);

        // Assert and Verify correct method calls
        mockMapper.Verify(m => m.Map<IEnumerable<DamageDoneGeneral>>(It.IsAny<IEnumerable<DamageDoneGeneralDto>>()), Times.Once);
        mockRepository.Verify(r => r.CreateBatchAsync(It.IsAny<IEnumerable<DamageDoneGeneral>>(), It.IsAny<CancellationToken>()), Times.Once);
        mockMapper.Verify(m => m.Map<IEnumerable<DamageDoneGeneralDto>>(It.IsAny<IEnumerable<DamageDoneGeneral>>()), Times.Once);
    }
}
