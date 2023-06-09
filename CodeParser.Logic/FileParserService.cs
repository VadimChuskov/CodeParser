﻿using System.Text.RegularExpressions;
using CodeParser.Domain;
using CodeParser.Domain.Enums;
using CodeParser.Logic.Interfaces;
using static System.Text.RegularExpressions.Regex;
using Object = CodeParser.Domain.Object;
using Enum = CodeParser.Domain.Enum;

namespace CodeParser.Logic;

public class FileParserService : IFileParserService
{
    private const string ObjectNameTypePattern = @"(?<type>(class|enum)) (?<name>\w*)";
    private const string NamespacePattern = @"namespace (?<namespace>[\w|\.]*)";
    private const string CommonUsingPattern = @"using (static)? ?(?<namespace>[\w|\.]*);";
    private const string SpecificUsingPattern = @"using (?<type>[\w|\.]*) = (?<namespace>[\w|\.]*);";
    private const string MethodSignaturePattern = @"(?<accessModifier>(public|private|protected|internal))\s*(?<isAsync>(async)?)\s*(?<returnedType>\S*)\s*(?<name>\S*)\((?<params>((\s*\w*\??,?)*))\)";
    private const string MethodParameterPattern = @"(?<type>(\w)*) (?<name>(\w)*)";

    public async Task<IEnumerable<Object>> ParseAsync(string path)
    {
        var result = new List<Object>();
        if (!File.Exists(path))
        {
            return result;
        }

        var text = await File.ReadAllTextAsync(path);

        var commonUsings = GetCommonUsings(text);
        //var specificUsings = GetSpecificUsings(text);

        var fileNamespace = GetNamespace(text);

        var matches = Matches(text, ObjectNameTypePattern, RegexOptions.None);

        foreach (Match match in matches)
        {
            var name = match.Groups["name"].Value;

            switch (match.Groups["type"].Value)
            {
                case "class":
                    var classToParsing = new Class(name, fileNamespace, path);
                    ParseClass(classToParsing, text);
                    result.Add(classToParsing);
                    break;
                case "enum":
                    result.Add(new Enum(name, fileNamespace, path));
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

        throw new ArgumentException("using??");
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
                var method = new Method(match.Groups["name"].Value, classToParsing.Namespace, classToParsing.Path, classToParsing);
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

                method.Result = new Object(match.Groups["returnedType"].Value, null, null);
                classToParsing.Methods.Add(method);
            }
        }
    }
}