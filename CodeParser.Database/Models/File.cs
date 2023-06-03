using Microsoft.EntityFrameworkCore;

namespace CodeParser.Database.Models;

[Index(nameof(Name))]
public class File
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string Hash { get; set; }
    public Namespace Namespace { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastUpdate { get; set; }
}