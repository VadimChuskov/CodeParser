using CodeParser.DataAccess.Interfaces;
using CodeParser.Database;

namespace CodeParser.DataAccess;

public class FileRepository : IFileRepository
{
    public void TestConnection()
    {
        using var db = new DataContext();

        var test = db.Files.Any();
    }
}