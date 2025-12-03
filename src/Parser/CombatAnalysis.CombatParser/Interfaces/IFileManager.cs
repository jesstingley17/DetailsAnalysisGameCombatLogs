namespace CombatAnalysis.CombatParser.Interfaces;

public interface IFileManager
{
    StreamReader StreamReader(string path);

    Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);
}
