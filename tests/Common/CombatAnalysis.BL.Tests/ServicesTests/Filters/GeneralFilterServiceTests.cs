using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Services.Filters;
using CombatAnalysis.BL.Tests.Factory;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces.Filters;
using Moq;

namespace CombatAnalysis.BL.Tests.ServicesTests.Filters;

public class GeneralFilterServiceTests
{
    [Fact]
    public async Task GetTargetNamesByCombatPlayerIdAsync_CollectionOfTargets_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;

        var targets = new List<string> { "Boss", "Creator 1" };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId)).ReturnsAsync(targets);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetTargetNamesByCombatPlayerIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetTargetNamesByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;

        var targets = new List<string> { "Boss", "Creator 1" };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId)).ReturnsAsync(targets);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetTargetNamesByCombatPlayerIdAsync(combatPlayerId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetTargetNamesByCombatPlayerIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CountTargetsByCombatPlayerIdAsync_CountOfEntity_ShouldCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Boss";
        const int count = 5;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountTargetByCombatPlayerIdAsync(combatPlayerId, target)).ReturnsAsync(count);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountTargetByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CountTargetsByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string target = "Boss";
        const int count = 5;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountTargetByCombatPlayerIdAsync(combatPlayerId, target)).ReturnsAsync(count);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CountTargetsByCombatPlayerIdAsync(combatPlayerId, target));

        // Verify correct method calls
        mockRepository.Verify(r => r.CountTargetByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByTargetAsync_CollectionOfTargets_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Boss";
        const int page = 1;
        const int pageSize = 5;

        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();
        var damages = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetByTargetAsync(combatPlayerId, target, page, pageSize)).ReturnsAsync(damages);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByTargetAsync(combatPlayerId, target, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByTargetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByTargetAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string target = "Boss";
        const int page = 1;
        const int pageSize = 5;

        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();
        var damages = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetByTargetAsync(combatPlayerId, target, page, pageSize)).ReturnsAsync(damages);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByTargetAsync(combatPlayerId, target, page, pageSize));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByTargetAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetTargetValueByCombatPlayerIdAsync_ValueByTarget_ShouldCalculateValueByTargetAndReturnIt()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string target = "Boss";
        const int value = 213414;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target)).ReturnsAsync(value);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target);

        // Assert
        Assert.Equal(value, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.GetTargetValueByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetTargetValueByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotCalculateValueByTargetAndReturnIt()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string target = "Boss";
        const int value = 213414;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target)).ReturnsAsync(value);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetTargetValueByCombatPlayerIdAsync(combatPlayerId, target));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetTargetValueByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetCreatorNamesByCombatPlayerIdAsync_CollectionOfCreators_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;

        var creators = new List<string> { "Boss", "Creator 1" };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId)).ReturnsAsync(creators);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetCreatorNamesByCombatPlayerIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetCreatorNamesByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;

        var creators = new List<string> { "Boss", "Creator 1" };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId)).ReturnsAsync(creators);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetCreatorNamesByCombatPlayerIdAsync(combatPlayerId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetCreatorNamesByCombatPlayerIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CountCreatorByCombatPlayerIdAsync_CountOfEntity_ShouldCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string creator = "Boss";
        const int count = 5;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator)).ReturnsAsync(count);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountCreatorByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CountCreatorByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string creator = "Boss";
        const int count = 5;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator)).ReturnsAsync(count);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CountCreatorByCombatPlayerIdAsync(combatPlayerId, creator));

        // Verify correct method calls
        mockRepository.Verify(r => r.CountCreatorByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetByCreatorAsync_CollectionOfTargets_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string creator = "Boss";
        const int page = 1;
        const int pageSize = 5;

        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();
        var damages = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetByCreatorAsync(combatPlayerId, creator, page, pageSize)).ReturnsAsync(damages);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetByCreatorAsync(combatPlayerId, creator, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCreatorAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetByCreatorAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string creator = "Boss";
        const int page = 1;
        const int pageSize = 5;

        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();
        var damages = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetByCreatorAsync(combatPlayerId, creator, page, pageSize)).ReturnsAsync(damages);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetByCreatorAsync(combatPlayerId, creator, page, pageSize));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetByCreatorAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetSpellNamesByCombatPlayerIdAsync_CollectionOfCreators_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;

        var spells = new List<string> { "Spell 1", "Spell 2" };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId)).ReturnsAsync(spells);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetSpellNamesByCombatPlayerIdAsync(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetSpellNamesByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;

        var spells = new List<string> { "Spell 1", "Spell 2" };

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId)).ReturnsAsync(spells);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetSpellNamesByCombatPlayerIdAsync(combatPlayerId));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetSpellNamesByCombatPlayerIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task CountSpellByCombatPlayerIdAsync_CountOfEntity_ShouldCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string spell = "Sepll 1";
        const int count = 5;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell)).ReturnsAsync(count);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell);

        // Assert
        Assert.Equal(count, result);

        // Verify correct method calls
        mockRepository.Verify(r => r.CountSpellByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CountSpellByCombatPlayerIdAsync_ThrowArgumentOutOfRangeException_ShouldNotCalculateEntityAndReturnCount()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string spell = "Spell 1";
        const int count = 5;

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockRepository.Setup(m => m.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell)).ReturnsAsync(count);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.CountSpellByCombatPlayerIdAsync(combatPlayerId, spell));

        // Verify correct method calls
        mockRepository.Verify(r => r.CountSpellByCombatPlayerIdAsync(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetBySpellAsync_CollectionOfTargets_ShouldReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 1;
        const string spell = "Spell";
        const int page = 1;
        const int pageSize = 5;

        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();
        var damages = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetBySpellAsync(combatPlayerId, spell, page, pageSize)).ReturnsAsync(damages);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act
        var result = await service.GetBySpellAsync(combatPlayerId, spell, page, pageSize);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(3, result.Count());

        // Verify correct method calls
        mockRepository.Verify(r => r.GetBySpellAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetBySpellAsync_ThrowArgumentOutOfRangeException_ShouldNotReturnAFewElementsInCollection()
    {
        // Arrange
        const int combatPlayerId = 0;
        const string spell = "Spell 1";
        const int page = 1;
        const int pageSize = 5;

        var damagesDto = DamageDoneTestDataFactory.CreateDtoCollection();
        var damages = DamageDoneTestDataFactory.CreateCollection();

        var mockMapper = new Mock<IMapper>();
        var mockRepository = new Mock<IGeneralFilterRepository<DamageDone>>();

        mockMapper.Setup(m => m.Map<IEnumerable<DamageDoneDto>>(damages)).Returns(damagesDto);

        mockRepository.Setup(m => m.GetBySpellAsync(combatPlayerId, spell, page, pageSize)).ReturnsAsync(damages);

        var service = new GeneralFilterService<DamageDoneDto, DamageDone>(mockRepository.Object, mockMapper.Object);

        // Act and Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetBySpellAsync(combatPlayerId, spell, page, pageSize));

        // Verify correct method calls
        mockRepository.Verify(r => r.GetBySpellAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
    }
}
