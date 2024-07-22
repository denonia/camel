using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Camel.Patcher;

public class Patcher
{
    public string FileName { get; }
    public string ResultFileName { get; }
    public string FromDomain { get; }
    public string ToDomain { get; }

    public Patcher(string fileName, string resultFileName, string fromDomain, string toDomain)
    {
        FileName = fileName;
        ResultFileName = resultFileName;
        FromDomain = fromDomain;
        ToDomain = toDomain;
    }

    public void Run()
    {
        if (!File.Exists(FileName))
        {
            Console.WriteLine("Source file not found");
            return;
        }
        
        var module = ModuleDefinition.ReadModule(FileName);

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