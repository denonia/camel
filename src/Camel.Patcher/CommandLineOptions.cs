using CommandLine;

namespace Camel.Patcher;

public class CommandLineOptions
{
    [Value(0, MetaName = "fileName", Required = true, HelpText = "Path to osu!.exe")]
    public required string FileName { get; set; }
    
    [Option('o', "out", HelpText = "Name of output file")]
    public string? ResultFileName { get; set; }
    
    [Option('f', "from", Default = "ppy.sh", HelpText = "Source url")]
    public required string FromDomain { get; set; }
    
    [Option('t', "to", Default = "allein.xyz", HelpText = "Destination url")]
    public required string ToDomain { get; set; }
    
    [Option('i', "ip", Default = "127.0.0.1", HelpText = "Destination IP address")]
    public required string ToIpAddress { get; set; }
}