namespace CodeParser.Domain;

public class Object : DomainEntity
{
    public Object(string name, File file)
    {
        Name = name;
        File = file;
    }

    public File File { get; }
    public IEnumerable<string> Usings { get; } = new List<string>();
}