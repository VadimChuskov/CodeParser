namespace CodeParser.DataAccess.Interfaces;

public interface IFileRepository
{
    public void TestConnection();
    public Task<IReadOnlyCollection<CodeParser.Domain.File>> FindAsync(string fileName);

    public Task AddAsync(CodeParser.Domain.File file);

    public Task UpdateAsync(Domain.File file);
}