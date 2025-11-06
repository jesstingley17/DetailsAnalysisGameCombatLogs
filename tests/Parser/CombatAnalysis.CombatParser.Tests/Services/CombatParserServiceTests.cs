using CombatAnalysis.CombatParser.Interfaces;
using CombatAnalysis.CombatParser.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;

namespace CombatAnalysis.CombatParser.Tests.Services;

public class CombatParserServiceTests
{
    [Fact]
    public async Task FileCheckAsync_ReturnsTrue_WhenFileContainsCombatLogVersion()
    {
        // Arrange
        var fakeFileContent = "6/8 20:42:39.739  COMBAT_LOG_VERSION,9,ADVANCED_LOG_ENABLED,1,BUILD_VERSION,2.5.4,PROJECT_ID,5";
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fakeFileContent));
        var reader = new StreamReader(stream);
        mockFileManager.Setup(fm => fm.StreamReader(It.IsAny<string>())).Returns(reader);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object);

        // Act
        var result = await service.FileCheckAsync("fakepath.txt");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task FileCheckAsync_ReturnsFalse_WhenFileIsEmpty()
    {
        // Arrange
        var fakeFileContent = string.Empty;
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();

        var stream = new MemoryStream(Encoding.UTF8.GetBytes(fakeFileContent));
        var reader = new StreamReader(stream);
        mockFileManager.Setup(fm => fm.StreamReader(It.IsAny<string>())).Returns(reader);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object);

        // Act
        var result = await service.FileCheckAsync("fakepath.txt");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task ParseAsync_ReturnsFalse_ParseDataFromAFewFiles()
    {
        // Arrange
        var mockFileManager = new Mock<IFileManager>();
        var mockLogger = new Mock<ILogger>();

        var fakeLines1 = new[] 
        {
            "9/8/2025 21:29:59.4433  ENCOUNTER_START,1501,\"Великая императрица Шек'зир\",3,10,1009,19",
            "9/8/2025 21:31:22.1000  SPELL_DAMAGE,\"Волако\",\"Страж племени Амани\",0xa48,0x0,26862,\"Коварный удар\",84842,86172,0,0,0,-1,0,0,0,131.84,1590.25,333,2.0246,71,1330,889,-1,1,0,0,0,1,nil,nil",
            "9/8/2025 21:34:51.1173  ENCOUNTER_END,1501,\"Великая императрица Шек'зир\",3,10,0",
        };

        // Return different content per file path
        mockFileManager
            .Setup(fm => fm.ReadAllLinesAsync("file1.txt", It.IsAny<CancellationToken>()))
            .ReturnsAsync(fakeLines1);

        var service = new CombatParserService(mockFileManager.Object, mockLogger.Object);

        var paths = new List<string> { "file1.txt" };
        var cancellationToken = CancellationToken.None;

        // Act
        await service.ParseAsync(paths, cancellationToken);

        // Assert
        mockFileManager.Verify(fm => fm.ReadAllLinesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
        Assert.NotEmpty(service.Combats);
        Assert.NotEmpty(service.CombatDetails);
    }
}
