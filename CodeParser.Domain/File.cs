namespace CodeParser.Domain;

public class File : DomainEntity
{
    public File(string name, string path, string hash, string fileNamespace)
    {
        Name = name;
        Path = path;
        Hash = hash;
        Namespace = fileNamespace;
    }
    public string Path { get; }
    public string Hash { get; }
    public string Namespace { get; }
    public IList<Object> Items { get; } = new List<Object>();
}