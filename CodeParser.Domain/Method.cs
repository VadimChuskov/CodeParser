namespace CodeParser.Domain;

public class Method : Object
{
    public Method(string name, string fileNamespace, string path, Object parent) 
        : base(name, fileNamespace, path)
    {
        Parent = parent;
    }

    //public IEnumerable<Parameter> Parameters { get; } = new List<Parameter>();
    public IList<string> Parameters { get; } = new List<string>();

    public Object Result { get; set; }

    public Object Parent { get; set; }

    public bool IsConstructor => String.IsNullOrEmpty(Name) 
                                 && Parent != null 
                                 && !string.IsNullOrEmpty(Parent.Name) 
                                 && Parent.Name == Result.Name;
}