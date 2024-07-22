using CommandLine;

namespace Camel.Patcher;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(o =>
            {
                var patcher = new Patcher(o.FileName, 
                    o.ResultFileName ?? ResultFileName(o.FileName, o.ToDomain), 
                    o.FromDomain, o.ToDomain);
                patcher.Run();
            });
    }

    private static string ResultFileName(string fileName, string domain)
    {
        var dir = Path.GetDirectoryName(fileName);
        var fnWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        var newFileName = $"{fnWithoutExtension}-{domain}{extension}";
        return string.IsNullOrEmpty(dir) ? newFileName : Path.Combine(dir, newFileName);
    }
}