using System.Text.RegularExpressions;
using CodeParser.Common;
using CodeParser.DataAccess;
using CodeParser.DataAccess.Interfaces;
using CodeParser.Domain;
using CodeParser.Domain.Enums;
using CodeParser.Logic.Interfaces;
using static System.Text.RegularExpressions.Regex;
using Object = CodeParser.Domain.Object;
using Enum = CodeParser.Domain.Enum;
using File = CodeParser.Domain.File;
using IoFile = System.IO.File;

namespace CodeParser.Logic;

public class FileParserService : IFileParserService
{
    private const string ObjectNameTypePattern = @"(?<type>(class|enum)) (?<name>\w*)";
    private const string NamespacePattern = @"namespace (?<namespace>[\w|\.]*)";
    private const string CommonUsingPattern = @"using (static)? ?(?<namespace>[\w|\.]*);";
    private const string SpecificUsingPattern = @"using (?<type>[\w|\.]*) = (?<namespace>[\w|\.]*);";
    private const string MethodSignaturePattern = @"(?<accessModifier>(public|private|protected|internal))\s*(?<isAsync>(async)?)\s*(?<returnedType>\S*)\s*(?<name>\S*)\((?<params>((\s*\w*\??,?)*))\)";
    private const string MethodParameterPattern = @"(?<type>(\w)*) (?<name>(\w)*)";

    private const string FieldSignaturePattern = @"(?<accessModifier>(public|private|protected|internal))\s*(?<type>[a-zA-Z<>]*)(?<nullable>(\?)?)\s*(?<name>\S*)";

    private readonly IFileRepository _fileRepository;
    
    static readonly SemaphoreSlim Semaphore = new(1,1);
    
    public FileParserService()
    {
        _fileRepository = new FileRepository();
    }
    
    public async Task<IEnumerable<Object>> ParseAsync(string path)
    {
        if (!Path.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        var hash = ShaManipulator.GetCheckSum(path);

        var fileName = Path.GetFileName(path);

        IReadOnlyCollection<File> files;
        
        await Semaphore.WaitAsync();
        try
        {
            files = await _fileRepository.FindAsync(fileName);
        }
        finally
        {
            Semaphore.Release();
        }

        var file = files.FirstOrDefault(f => f.Path.Equals(path));

        if (file != null && file.Hash.Equals(hash))
        {
            //
            return new List<Object>();
        }
        
        var result = new List<Object>();

        var text = await IoFile.ReadAllTextAsync(path);

        //var commonUsings = GetCommonUsings(text);
        //var specificUsings = GetSpecificUsings(text);

        var fileNamespace = GetNamespace(text);
        
        if (file != null)
        {
            file.UpdateHash(hash);
            
            await Semaphore.WaitAsync();
            try
            {
                await _fileRepository.UpdateAsync(file);
            }
            finally
            {
                Semaphore.Release();
            }
        }
        else
        {
            file = new File(fileName, path, hash, fileNamespace);
            await Semaphore.WaitAsync();
            try
            {
                await _fileRepository.AddAsync(file);
            }
            finally
            {
                Semaphore.Release();
            }
        }

        var matches = Matches(text, ObjectNameTypePattern, RegexOptions.None);
        
        foreach (Match match in matches)
        {
            var name = match.Groups["name"].Value;
        
            switch (match.Groups["type"].Value)
            {
                case "class":
                    var classToParsing = new Class(name, file);
                    ParseClass(classToParsing, text);
                    result.Add(classToParsing);
                    break;
                case "enum":
                    result.Add(new Enum(name, file));
                    break;
            }
        }

        return result;
    }

    private List<string> GetCommonUsings(string fileContent)
    {
        var matches = Matches(fileContent, CommonUsingPattern, RegexOptions.None);
        if (matches.Any())
        {
            return matches.Select(it => it.Groups["namespace"].Value).ToList();
        }

        return new List<string>();
    }

    private List<string> GetSpecificUsings(string fileContent)
    {
        var matches = Matches(fileContent, SpecificUsingPattern, RegexOptions.None);
        if (matches.Any())
        {
            return matches.Select(it => it.Groups["namespace"].Value).ToList();
        }

        throw new ArgumentException("using??");
    }

    private string GetNamespace(string fileContent)
    {
        var matches = Matches(fileContent, NamespacePattern, RegexOptions.None);
        if (matches.Count == 1)
        {
            return matches[0].Groups["namespace"].Value;
        }

        throw new ArgumentException("namespace??");
    }

    private void ParseClass(Class classToParsing, string text)
    {
        var name = classToParsing.Name;
    
        if (name.EndsWith("Service"))
        {
            classToParsing.ClassType = ClassType.Service;
        }
        else if (name.EndsWith("Dto") || name.EndsWith("Model"))
        {
            classToParsing.ClassType = ClassType.Model;
        }
    
        var matches = Matches(text, MethodSignaturePattern, RegexOptions.None);
        if (matches.Any())
        {
            foreach (Match match in matches)
            {
                var method = new Method(match.Groups["name"].Value, classToParsing.File, classToParsing);
                var paramsString = match.Groups["params"].Value;
                if (!string.IsNullOrEmpty(paramsString) && !paramsString.Equals("void"))
                {
                    var paramStringList = paramsString.Split(',');
                    //var order = 0;
                    foreach (var param in paramStringList)
                    {
                        method.Parameters.Add(param.Trim());
                    }
                }
    
                method.Result = new Object(match.Groups["returnedType"].Value, classToParsing.File);
                classToParsing.Methods.Add(method);
            }
        }
        
        matches = Matches(text, FieldSignaturePattern, RegexOptions.None);
        if (matches.Any())
        {
            foreach (Match match in matches)
            {
                var field = new Field(match.Groups["name"].Value, classToParsing.File, classToParsing)
                    {
                        Type = match.Groups["type"].Value,
                        Nullable = !string.IsNullOrEmpty(match.Groups["nullable"].Value)
                    };
    
                classToParsing.Fields.Add(field);
            }
        }
    }
}