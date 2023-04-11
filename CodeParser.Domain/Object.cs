namespace CodeParser.Domain;

public class Object
{
    public Object(string name, string fileNamespace, string path)
    {
        Name = name;
        Path = path;
        Namespace = fileNamespace;
    }

    public string Name { get; set; }
    public string Path { get; set; }

    public string Namespace { get; set; }

    public IEnumerable<string> Usings { get; } = new List<string>();
}