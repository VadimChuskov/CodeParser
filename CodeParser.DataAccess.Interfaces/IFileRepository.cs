using File = CodeParser.Database.Models.File;

namespace CodeParser.DataAccess.Interfaces;

public interface IFileRepository
{
    public void TestConnection();
    public IReadOnlyCollection<File> Find(string fileName);
    public Task<IReadOnlyCollection<File>> FindAsync(string fileName);

    public Task AddAsync(CodeParser.Domain.File file);
    public void Add(Domain.File file);
}