namespace CodeParser.Domain;

public class Field : Object
{
    private readonly List<string> _valueTypes = new() { "bool", "int", "string", "float" };

    public Field(string name, File file, Object parent) 
        : base(name, file)
    {
        Parent = parent;
    }
    public Object Parent { get; set; }
    public string Type { get; set; }
    public bool Nullable { get; set; }
    public bool IsValueType => _valueTypes.Contains(Type);
}