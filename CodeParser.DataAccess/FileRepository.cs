using CodeParser.DataAccess.Interfaces;
using CodeParser.Database;
using CodeParser.Database.Models;
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

        var test = db.File.Any();
    }

    public async Task<IReadOnlyCollection<Domain.File>> FindAsync(string fileName)
    {
        var entities = await _dataContext.File
            .AsNoTracking()
            .Include(file => file.Namespace)
            .Where(f =>  f.Name.Equals(fileName))
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task AddAsync(Domain.File file)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync();
        
        var namespaceEntity = await UpdateNamespace(file.Namespace);

        var entity = await _dataContext.File.AddAsync(new File
        {
            Name = file.Name,
            Path = file.Path,
            Hash = file.Hash,
            Namespace = namespaceEntity,
            Created = DateTime.Now
        });
        
        await _dataContext.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }

    public async Task UpdateAsync(Domain.File file)
    {
        await using var transaction = await _dataContext.Database.BeginTransactionAsync();

        var namespaceEntity = await UpdateNamespace(file.Namespace);

        //var entity = await _dataContext.File.FindAsync()
        
        _dataContext.File.Update(new File
        {
            Id = file.Id.GetValueOrDefault(),
            Name = file.Name,
            Path = file.Path,
            Hash = file.Hash,
            Namespace = namespaceEntity,
            //Created = file.Created,
            LastUpdate = DateTime.Now
        });
        
        await _dataContext.SaveChangesAsync();
        
        await transaction.CommitAsync();
    }

    private async Task<Namespace> UpdateNamespace(string fileNamespace)
    {
        var sqlCommand = $"INSERT OR IGNORE INTO [Namespace] ([Name]) VALUES ('{fileNamespace}')";

        await _dataContext.Database
            .ExecuteSqlRawAsync(sqlCommand);

        return await _dataContext.Namespace.SingleAsync(n => n.Name.Equals(fileNamespace));
    }

    private Domain.File MapToDomain(File file)
    {
        return new Domain.File(file.Name, file.Path, file.Hash, file.Namespace.Name);
    }
}