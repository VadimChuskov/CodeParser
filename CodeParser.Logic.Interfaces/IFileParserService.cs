using Object = CodeParser.Domain.Object;

namespace CodeParser.Logic.Interfaces;

public interface IFileParserService
{
    Task<IEnumerable<Object>> ParseAsync(string path);
}