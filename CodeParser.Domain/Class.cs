using CodeParser.Domain.Enums;

namespace CodeParser.Domain;

public class Class : Object
{
    public Class(string name, File file) : base(name, file)
    {
    }

    public ClassType ClassType { get; set; }

    public IList<Method> Methods { get; } = new List<Method>();
    public IList<Field> Fields { get; } = new List<Field>();
}