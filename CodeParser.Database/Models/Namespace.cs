using Microsoft.EntityFrameworkCore;

namespace CodeParser.Database.Models;

[Index(nameof(Name), IsUnique = true)]
public class Namespace
{
    public int Id { get; set; }
    public string Name { get; set; }
}