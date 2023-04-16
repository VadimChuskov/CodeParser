namespace CodeParser.Domain;

public class Object
{
    public Object(string name, string fileNamespace, string path)
    {
        Name = name;
        Path = path;
        Namespace = fileNamespace;
    }

    public string Name { get; }
    public string Path { get; }

    public string Namespace { get; }

    public IEnumerable<string> Usings { get; } = new List<string>();
}