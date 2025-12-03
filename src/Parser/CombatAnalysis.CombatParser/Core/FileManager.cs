using CombatAnalysis.CombatParser.Interfaces;

namespace CombatAnalysis.CombatParser.Core;

public class FileManager : IFileManager
{
    public StreamReader StreamReader(string path)
        => new(path);

    public Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken)
        => File.ReadAllLinesAsync(path, cancellationToken);
}
