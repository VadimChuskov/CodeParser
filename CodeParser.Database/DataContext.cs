using CodeParser.Database.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CodeParser.Database;

public class DataContext : DbContext
{
    public DataContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = Path.Join(path, "CodeParser.db");
    }
    
    public string DbPath { get; }


    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ClassType>()
            .Property(e => e.Id)
            .HasConversion<int>();

        modelBuilder
            .Entity<ClassType>().HasData(
                Enum.GetValues(typeof(ClassTypeId))
                    .Cast<ClassTypeId>()
                    .Select(e => new ClassType()
                    {
                        Id = e,
                        Name = e.ToString()
                    })
            );
    }

    public DbSet<Models.File> File { get; set; }
    public DbSet<Models.Namespace> Namespace { get; set; }
}