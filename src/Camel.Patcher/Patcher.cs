using System.Net;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Camel.Patcher;

public class Patcher
{
    private static readonly long[] BanchoIps = 
    [
        1514805042L, // 50.23.74.90
        1565136690L  // 50.23.74.93
    ];
    
    public string FileName { get; }
    public string ResultFileName { get; }
    public string FromDomain { get; }
    public string ToDomain { get; }
    public long ToIpAddress { get; }

    public Patcher(string fileName, string resultFileName, string fromDomain, string toDomain, string toIpAddress)
    {
        FileName = fileName;
        ResultFileName = resultFileName;
        FromDomain = fromDomain;
        ToDomain = toDomain;
        ToIpAddress = IPAddress.Parse(toIpAddress).Address;
    }

    public void Run()
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine("Source file not found");
            return;
        }
        
        var module = ModuleDefinition.ReadModule(FileName);

        var ipInstructions = module.Types
            .SelectMany(t => t.Methods)
            .Where(m => m.HasBody)
            .SelectMany(m => m.Body.Instructions)
            .Where(i => i.OpCode == OpCodes.Ldc_I8)
            .Where(i => BanchoIps.Contains((i.Operand as long?)!.Value))
            .ToList();
        
        foreach (var instruction in ipInstructions)
        {
            Console.WriteLine($"Found Bancho IP: {instruction.Operand}");
        
            instruction.Operand = ToIpAddress;
        }
        
        var stringInstructions = module.Types
            .SelectMany(t => t.Methods)
            .Where(m => m.HasBody)
            .SelectMany(m => m.Body.Instructions)
            .Where(i => i.OpCode == OpCodes.Ldstr)
            .Where(s => (s.Operand as string)!.Contains(FromDomain))
            .ToList();

        Console.WriteLine($"Found {stringInstructions.Count} instances of {FromDomain}");

        if (stringInstructions.Count == 0)
            return;

        foreach (var instruction in stringInstructions)
        {
            var str = (instruction.Operand as string)!;
            Console.WriteLine(str);
            instruction.Operand = str.Replace(FromDomain, ToDomain);
        }

        module.Write(ResultFileName);
    }
}