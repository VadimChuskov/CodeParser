using CodeParser.Logic.Interfaces;
using File = CodeParser.Domain.File;

namespace CodeParser.Logic;

public class FolderParserService : IFolderParserService
{
    private readonly IFileParserService _fileParserService;
    public FolderParserService()
    {
        _fileParserService = new FileParserService();
    }
    
    public async Task<IEnumerable<File>> ParseAsync(string path)
    {
        if (!Path.Exists(path))
        {
            throw new DirectoryNotFoundException(path);
        }

        var files = Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories)
            .Where(fp => 
                !fp.Contains("\\obj\\") 
                && !fp.Contains("\\bin\\")
                && !fp.Contains("Tests"))
            .ToList();

        var availableProcessorCount = Environment.ProcessorCount - 3;
        //var availableProcessorCount = 4;

        var filesPerProcessor = (files.Count / availableProcessorCount) + 1;
        
        var workerThreads = new Thread[availableProcessorCount];

        for (var i = 0; i < availableProcessorCount; i++)
        {
            workerThreads[i] = new Thread(Worker);
            workerThreads[i].Start(files.Skip(i * filesPerProcessor).Take(filesPerProcessor));
            workerThreads[i].Join();
        }

        return new List<File>();
    }

    private async void Worker(object? obj)
    {
        if (obj is IEnumerable<string> list)
        {
            var fileParserService = new FileParserService();
            var tt = Thread.CurrentThread;
            foreach (var item in list)
            {
                await fileParserService.ParseAsync(item);
            }
        }
    }
}