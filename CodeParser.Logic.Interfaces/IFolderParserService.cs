using File = CodeParser.Domain.File;

namespace CodeParser.Logic.Interfaces;

public interface IFolderParserService
{
    Task<IEnumerable<File>> ParseAsync(string path);
}