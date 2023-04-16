using System.Transactions;
using CodeParser.DataAccess.Interfaces;
using CodeParser.Database;
using Microsoft.EntityFrameworkCore;
using File = CodeParser.Database.Models.File;

namespace CodeParser.DataAccess;

public class FileRepository : IFileRepository
{
    private readonly DataContext _dataContext;
    public FileRepository()
    {
        _dataContext = new DataContext();
    }
    
    public void TestConnection()
    {
        using var db = new DataContext();

        var test = db.Files.Any();
    }

    public async Task<IReadOnlyCollection<File>> FindAsync(string fileName)
    {
        var entities = await _dataContext.Files
            .AsNoTracking()
            .Where(f =>  f.Name.Equals(fileName))
            .ToListAsync();

        return entities.AsReadOnly();
    }

    public IReadOnlyCollection<File> Find(string fileName)
    {
        var entities = _dataContext.Files
            .AsNoTracking()
            .Where(f =>  f.Name.Equals(fileName))
            .ToList();

        return entities.AsReadOnly();
    }

    public async Task AddAsync(Domain.File file)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var entity = await _dataContext.Files.AddAsync(new File
        {
            Name = file.Name,
            Path = file.Path,
            Hash = file.Hash,
            Created = DateTime.Now
        });
        
        scope.Complete();
    }
    
    public void Add(Domain.File file)
    {
        using var transaction = _dataContext.Database.BeginTransaction();

        var entity = _dataContext.Files.Add(new File
        {
            Name = file.Name,
            Path = file.Path,
            Hash = file.Hash,
            Created = DateTime.Now
        });
        
        _dataContext.SaveChanges();
        
        transaction.Commit();
    }
}