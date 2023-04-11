using CodeParser.Domain.Enums;

namespace CodeParser.Domain;

public class Class : Object
{
    public Class(string name, string fileNamespace, string path)
        : base(name, fileNamespace, path)
    {
    }

    public ClassType ClassType { get; set; }

    public IList<Method> Methods { get; } = new List<Method>();
}