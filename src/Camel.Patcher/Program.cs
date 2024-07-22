using CommandLine;

namespace Camel.Patcher;

class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<CommandLineOptions>(args)
            .WithParsed(o =>
            {
                var patcher = new Patcher(o.FileName, o.ResultFileName, o.FromDomain, o.ToDomain);
                patcher.Run();
            });
    }
}