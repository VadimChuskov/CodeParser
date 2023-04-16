namespace CodeParser.Domain;

public class File : Object
{
    public File(string name, string fileNamespace, string path, string hash) 
        : base(name, fileNamespace, path)
    {
        Hash = hash;
    }

    public string Hash { get; }
    public IList<Object> Items { get; } = new List<Object>();
}